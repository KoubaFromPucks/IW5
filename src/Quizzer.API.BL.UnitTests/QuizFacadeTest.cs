using Quizzer.Common.Models;
using System.Linq.Expressions;

namespace Quizzer.API.BL.UnitTests; 
public class QuizFacadeTest : TestBase{
    private Mock<IRepository<QuestionEntity>> _questionRepositoryMock = new(MockBehavior.Strict);
    private Mock<IRepository<QuizEntity>> _quizRepositoryMock = new(MockBehavior.Strict);
    private Mock<IRepository<UserEntity>> _userRepositoryMock = new(MockBehavior.Strict);

    [Theory]
    [InlineData("25.10.2023 10:00:00", "25.10.2023 11:00:00", "25.10.2023 11:00:01")]
    [InlineData("25.10.2023 10:00:00", "25.10.2023 11:00:00", "27.10.2023 11:00:00")]
    [InlineData("25.10.2023 10:00:00", "25.10.2023 09:59:59", "24.10.2023 11:00:00")]
    [InlineData("25.10.2023 10:00:00", "22.10.2023 11:00:00", "23.10.2023 11:00:00")]
    [InlineData("25.10.2023 10:00:00", "30.10.2023 11:00:00", "27.10.2023 11:00:00")]
    public void SaveQuiz_In_Bad_Time_ThrowsException(string quizStart, string quizEnd, string now) {
        // Arrange
        QuizFacade quizFacade = GetFacadeWithForbiddenDependencyCalls();
        var quizDetailModel = new QuizDetailModel {
            Id = Guid.NewGuid(),
            StartTime = ParseDateTime(quizStart),
            EndTime = ParseDateTime(quizEnd),
            Name = "Some name"
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => quizFacade.Save(quizDetailModel, ParseDateTime(now)));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData("\n")]
    [InlineData("\r")]
    [InlineData("\t")]
    [InlineData("\a")]
    [InlineData("\b")]
    [InlineData("\0")]
    [InlineData("\f")]
    [InlineData("\v")]
    [InlineData("\f\v \t \a\b \b\f\0 \v \a")]
    public void SaveQuiz_With_Bad_Name_ThrowsException(string name) {
        // Arrange
        QuizFacade quizFacade = GetFacadeWithForbiddenDependencyCalls();
        var quizDetailModel = new QuizDetailModel {
            Id = Guid.NewGuid(),
            StartTime = new DateTime(2023, 10, 10, 00, 00, 00),
            EndTime = new DateTime(2024, 10, 10, 00, 00, 00),
            Name = name
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => quizFacade.Save(quizDetailModel, new DateTime(2022, 10, 10, 00, 00, 00)));
    }

    [Theory]
    [InlineData("    Name     ", "Name", false)]
    [InlineData("\tQuiz\n\tName  \a   ", "Quiz\n\tName", false)]
    [InlineData("\n   \tNested\0\v\f", "Nested", false)]
    [InlineData("Quiz", "Quiz", false)]
    [InlineData("\0\a\r\bBritain\0\a\r\b", "Britain", false)]
    [InlineData("\0\a\r\f\b\v\tLotr\f\v\0\a\r\b\t", "Lotr", false)]
    [InlineData("    Name     ", "Name", true)]
    [InlineData("\tQuiz\n\tName  \a   ", "Quiz\n\tName", true)]
    [InlineData("\n   \tNested\0\v\f", "Nested", true)]
    [InlineData("Quiz", "Quiz", true)]
    [InlineData("\0\a\r\bBritain\0\a\r\b", "Britain", true)]
    [InlineData("\0\a\r\f\b\v\tLotr\f\v\0\a\r\b\t", "Lotr", true)]
    public void SaveQuiz_With_Ok_Name_CallSaveWithTrimedName(string untrimedName, string expectedName, bool exists) {
        // Arrange
        QuizFacade quizFacade = GetFacadeWithForbiddenDependencyCalls();
        string savedName = "";
        var quizDetailModel = new QuizDetailModel {
            Id = Guid.NewGuid(),
            StartTime = new DateTime(2023, 10, 10, 00, 00, 00),
            EndTime = new DateTime(2024, 10, 10, 00, 00, 00),
            Name = untrimedName
        };

        _quizRepositoryMock.Setup(rep => rep.Exists(quizDetailModel.Id)).Returns(exists);
        _quizRepositoryMock.Setup(rep => rep.Update(It.IsAny<QuizEntity>()))
            .Callback((QuizEntity ent) => savedName = ent.Name)
            .Returns(quizDetailModel.Id);
        _quizRepositoryMock.Setup(rep => rep.Insert(It.IsAny<QuizEntity>()))
            .Callback((QuizEntity ent) => savedName = ent.Name)
            .Returns(quizDetailModel.Id);

        // Act
        quizFacade.Save(quizDetailModel, new DateTime(2022, 10, 10, 00, 00, 00));

        // Assert
        if (exists) {
            _quizRepositoryMock.Verify(rep => rep.Update(It.IsAny<QuizEntity>()), Times.Once());
        } else {
            _quizRepositoryMock.Verify(rep => rep.Insert(It.IsAny<QuizEntity>()), Times.Once());
        }

        _uowMock.Verify(uow => uow.Commit(), Times.Once());
        Assert.Equal(expectedName, savedName);
    }

    [Theory]
    [InlineData("10.1.2023 10:0:0", "20.1.2023 12:10:0", "20.1.2023 12:10:1")]
    [InlineData("15.2.2023 10:0:0", "30.7.2023 11:20:0", "11.1.2024 16:20:0")]
    [InlineData("20.3.2023 10:0:0", "20.5.2023 19:30:0", "20.3.2023 9:59:59")]
    [InlineData("25.4.2023 10:0:0", "21.6.2023 13:33:4", "29.6.2023 4:50:38")]
    public void StartQuiz_Quiz_Is_Not_Opened_ThrowsException(string quizStart, string quizEnd, string now) {
        // Arrange
        QuizFacade quizFacade = GetFacadeWithForbiddenDependencyCalls();
        var quizEntity = new QuizEntity {
            Id = Guid.NewGuid(),
            StartTime = ParseDateTime(quizStart),
            EndTime = ParseDateTime(quizEnd),
            Name = "Quiz"
        };
        var userEntity = new UserEntity { Id = Guid.NewGuid(), Name = "My name" };
        Mock<IRepository<CompletedQuizEntity>> completedQuizRepositoryMock = new(MockBehavior.Loose);

        _uowMock.Setup(uow => uow.GetRepository<CompletedQuizEntity>()).Returns(completedQuizRepositoryMock.Object);

        _userRepositoryMock.Setup(rep => rep.GetById(It.IsAny<Guid>())).Returns(userEntity);
        _quizRepositoryMock.Setup(rep => rep.GetById(It.IsAny<Guid>())).Returns(quizEntity);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => quizFacade.StartQuiz(userEntity.Id, quizEntity.Id, ParseDateTime(now)));
    }

    [Theory]
    [InlineData("10.1.2023 10:0:0", "20.1.2023 12:10:0", "15.1.2023 12:10:1")]
    [InlineData("15.2.2023 10:0:0", "30.7.2023 11:20:0", "19.2.2023 16:20:0")]
    [InlineData("20.3.2023 10:0:0", "20.5.2023 19:30:0", "29.3.2023 9:59:59")]
    [InlineData("25.4.2023 10:0:0", "21.6.2023 13:33:4", "28.5.2023 4:50:38")]
    public void StartQuiz_Ok_CallCommit(string quizStart, string quizEnd, string now) {
        // Arrange
        QuizFacade quizFacade = GetFacadeWithForbiddenDependencyCalls();
        var quizEntity = new QuizEntity {
            Id = Guid.NewGuid(),
            StartTime = ParseDateTime(quizStart),
            EndTime = ParseDateTime(quizEnd),
            Name = "Quiz"
        };
        var userEntity = new UserEntity { Id = Guid.NewGuid(), Name = "My name" };
        Mock<IRepository<CompletedQuizEntity>> completedQuizRepositoryMock = new(MockBehavior.Loose);

        _uowMock.Setup(uow => uow.GetRepository<CompletedQuizEntity>()).Returns(completedQuizRepositoryMock.Object);
        completedQuizRepositoryMock.Setup(rep => rep.Insert(It.IsAny<CompletedQuizEntity>()));

        _userRepositoryMock.Setup(rep => rep.GetById(It.IsAny<Guid>())).Returns(userEntity);
        _quizRepositoryMock.Setup(rep => rep.GetById(It.IsAny<Guid>())).Returns(quizEntity);

        // Act
        quizFacade.StartQuiz(userEntity.Id, quizEntity.Id, ParseDateTime(now));

        // Assert
        completedQuizRepositoryMock.Verify(rep => rep.Insert(It.IsAny<CompletedQuizEntity>()), Times.Once());
        _uowMock.Verify(uow => uow.Commit(), Times.Once());
    }

    [Theory]
    [InlineData("Great Britaion", "", "", "", false)]
    [InlineData("Great Britain", "", "Great Britain", "Some sequence of Words", true)]
    [InlineData("Great risk at Britain", "", "Great Britain", "Some sequence of Words", true)]
    [InlineData("What you know about great britain?", "X", "Great Britain", "Some sequence of Words", true)]
    [InlineData("X", "some sequence of words", "Great Britain", "Some sequence of Words", true)]
    [InlineData("", "words sequence sth SOME something of nothing", "Great Britain", "Some sequence of Words", true)]
    [InlineData("Great risk at Britain", "Unknown descritpion", "Great Britain", "Some sequence of Words", true)]
    [InlineData("What you know about great britain?", "some sequence of words", "Great Britain", "Some sequence of Words", true)]
    [InlineData("Great Britain", "some sequence of words", "Great Britain", "Some sequence of Words", true)]
    [InlineData("Something different", "words sequence sth SOME something of nothing", "Great Britain", "Some sequence of Words", true)]
    [InlineData("", "somesequenceofwords", "Great Britain", "Some sequence of Words", true)]
    [InlineData("GreatBritain risk", "", "Great Britain", "Some sequence of Words", true)]
    [InlineData("X", "GreatBritain risk", "Great Britain", "Some sequence of Words", false)]
    [InlineData("Some sequence of Words", "Great Britain", "Great Britain", "Some sequence of Words", false)]
    [InlineData("Here is a Great tower", "", "Great Britain", "Some sequence of Words", false)]
    [InlineData("Not about Britain but France", "", "Great Britain", "Some sequence of Words", false)]
    [InlineData("France", "", "Great Britain", "Some sequence of Words", false)]
    [InlineData("Programming", "", "Great Britain", "Some sequence of Words", false)]
    [InlineData("", "What you know about FIT?", "Great Britain", "Some sequence of Words", false)]
    [InlineData("Sth", "", "AAA", "", false)]
    public void GetQuizByContent_ExpectSelected(string quizName, string quizDescription, string searchName, 
        string searchDescription, bool shouldBeFound) {
        // Arrange
        QuizFacade quizFacade = GetFacadeWithForbiddenDependencyCalls();
        var quizzes = new List<QuizEntity>() {
            new QuizEntity{
                Id = new Guid(), 
                EndTime = DateTime.Now.AddDays(2), 
                StartTime = DateTime.Now.AddDays(5), 
                Name = quizName, 
                Description = quizDescription}
        };

        _quizRepositoryMock.Setup(rep => rep.GetByPredicate(It.IsAny<Expression<Func<QuizEntity, bool>>>(), It.IsAny<string[]>()))
                           .Returns<Expression<Func<QuizEntity, bool>>, string[]>((predicate, nav) => quizzes.Where(predicate.Compile()));

        // Act 
        IEnumerable<QuizListModel> quizListModels = quizFacade.GetQuizByContent(searchName, searchDescription);

        // Assert
        Assert.NotNull(quizListModels);
        Assert.Equal(shouldBeFound? 1:0, quizListModels.Count());
    }

    private void SetUpMocksBase() {
        _uowMock.Setup(uow => uow.GetRepository<QuestionEntity>()).Returns(_questionRepositoryMock.Object);
        _uowMock.Setup(uow => uow.GetRepository<QuizEntity>()).Returns(_quizRepositoryMock.Object);
        _uowMock.Setup(uow => uow.GetRepository<UserEntity>()).Returns(_userRepositoryMock.Object);

        _mapperMock.Setup(map => map.Map<QuizEntity>(It.IsAny<QuizDetailModel>()))
                   .Returns<QuizDetailModel>(a => new QuizEntity { 
                       Id = a.Id, 
                       EndTime = a.EndTime, 
                       StartTime = a.StartTime, 
                       Name = a.Name,
                       Description = a.Description
                   });
        
        _mapperMock.Setup(map => map.Map<QuizListModel>(It.IsAny<QuizEntity>()))
                   .Returns<QuizEntity>(a => new QuizListModel{
                       Id = a.Id,
                       EndTime = a.EndTime,
                       StartTime = a.StartTime,
                       Name = a.Name,
                       Description = a.Description
                   });

        _quizRepositoryMock.Setup(rep => rep.GetById(It.IsAny<Guid>(), It.IsAny<string[]>())).Returns<Guid, string[]>((a, b) => null);
    }

    private QuizFacade GetFacadeWithForbiddenDependencyCalls() {
        SetUpMocksBase();
        var facade = new QuizFacade(_uowFactoryMock.Object, _mapperMock.Object);
        return facade;
    }
}
