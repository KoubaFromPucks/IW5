using Quizzer.Common.Models;
using System.Linq.Expressions;

namespace Quizzer.API.BL.UnitTests; 
public class QuestionFacadeTest : TestBase {
    private Mock<IRepository<QuestionEntity>> _questionRepositoryMock = new(MockBehavior.Strict);
    private Mock<IRepository<QuizEntity>> _quizRepositoryMock = new(MockBehavior.Strict);
    private Mock<IQuizChecker> _quizChecker = new(MockBehavior.Strict);

    [Fact]
    public void GetDetailById_Sets_Question_Types_Sets_Correctly_AnswerType() {
        // Arrange
        _questionRepositoryMock = new Mock<IRepository<QuestionEntity>>(MockBehavior.Loose);
        QuestionFacade questionFacade = GetFacadeWithForbiddenDependencyCalls();

        var questionDetailModel = new QuestionDetailModel {
            Id = Guid.NewGuid(),
            Text = "Name of quiz",
            QuestionType = AnswerFormat.SingleChoice,
            Answers = {
                new AnswerDetailModel {
                    Id = Guid.NewGuid(),
                    Text = "some text",
                    IsCorrect = false,
                    QuestionId = Guid.NewGuid()
                },
                new AnswerDetailModel {
                    Id = Guid.NewGuid(),
                    Text = "some text",
                    Type = AnswerFormat.OrderChoice,
                    IsCorrect = false,
                    QuestionId = Guid.NewGuid()
                },
                new AnswerDetailModel {
                    Id = Guid.NewGuid(),
                    Text = "some text",
                    Type = AnswerFormat.MultiChoice,
                    IsCorrect = false,
                    QuestionId = Guid.NewGuid()
                }
            }
        };

        _mapperMock.Setup(mapper => mapper.Map<QuestionDetailModel>(It.IsAny<QuestionEntity>()))
                   .Returns(questionDetailModel);
        _questionRepositoryMock.Setup(rep => rep.GetById(questionDetailModel.Id, It.IsAny<string[]>()))
                               .Returns(new QuestionEntity { 
                                   Id = questionDetailModel.Id,
                                   Text = questionDetailModel.Text,
                                   Type = questionDetailModel.QuestionType
                               });

        // Act 
        QuestionDetailModel? modelFromFacade = questionFacade.GetDetailById(questionDetailModel.Id);

        // Assert
        Assert.NotNull(modelFromFacade);
        Assert.Equal(3, questionDetailModel.Answers.Count);
        foreach (AnswerDetailModel answerModel in questionDetailModel.Answers)
            Assert.Equal(questionDetailModel.QuestionType, answerModel.Type);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("     ")]
    [InlineData("\t")]
    [InlineData("\n")]
    [InlineData("\r")]
    public void Save_Empty_Text_ThrowsException(string questionText) {
        // Arrange
        QuestionFacade questionFacade = GetFacadeWithForbiddenDependencyCalls();
        var questionDetailModel = new QuestionDetailModel { Id = Guid.NewGuid(), Text = questionText };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => questionFacade.Save(questionDetailModel, Guid.NewGuid(), DateTime.Now));
    }

    [Theory]
    [InlineData("25.10.2023 14:20:50", "25.10.2023 14:21:45", "25.10.2023 14:21:46")]
    [InlineData("25.10.2023 14:20:50", "29.12.2023 14:21:45", "25.10.2024 14:21:46")]
    [InlineData("25.10.2023 14:20:50", "29.12.2023 14:21:45", "25.10.2023 14:20:49")]
    [InlineData("25.10.2023 14:20:50", "29.12.2023 14:21:45", "25.10.2022 14:21:46")]
    public void AnswerQuestion_Quiz_Is_Not_Opened_ThrowsException(string quizStart, string quizEnd, string answerTime) {
        // Arrange 
        QuestionFacade questionFacade = GetFacadeWithForbiddenDependencyCalls();
        Mock<IRepository<SelectedAnswerEntity>> selectedAnswerRepositoryMock = new(MockBehavior.Strict);
        Mock<IRepository<AnswerEntity>> answerRepositoryMock = new(MockBehavior.Strict);
        Mock<IRepository<UserAnswerEntity>> userAnswerRepositoryMock = new(MockBehavior.Strict);

        var answerEntity = new AnswerEntity {
            Id = Guid.NewGuid(),
            IsCorrect = true,
            Text = "selected answer",
            Question = new QuestionEntity {
                Id = Guid.NewGuid(),
                Text = "quiz text",
                Type = AnswerFormat.SingleChoice,
                Quiz = new QuizEntity {
                    Id = Guid.NewGuid(),
                    StartTime = ParseDateTime(quizStart),
                    EndTime = ParseDateTime(quizEnd),
                    Name = "some name"
                }
            }
        };

        _uowMock.Setup(uow => uow.GetRepository<AnswerEntity>()).Returns(answerRepositoryMock.Object);
        answerRepositoryMock.Setup(rep => rep.GetById(
            It.IsAny<Guid>(),
            nameof(AnswerEntity.Question),
            $"{nameof(AnswerEntity.Question)}.{nameof(QuestionEntity.Quiz)}")
        ).Returns(answerEntity);

        _uowMock.Setup(uow => uow.GetRepository<SelectedAnswerEntity>()).Returns(selectedAnswerRepositoryMock.Object);
        _uowMock.Setup(uow => uow.GetRepository<UserAnswerEntity>()).Returns(userAnswerRepositoryMock.Object);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => questionFacade.AnswerQuestion(Guid.NewGuid(), Guid.NewGuid(), ParseDateTime(answerTime)));
    }

    [Theory]
    [InlineData("John Tolkien", "John Tolkien", true)]
    [InlineData("John Ronlad Reuel Tolkien", "John Tolkien", true)]
    [InlineData("Tolkien Johndeer John", "John Tolkien", true)]
    [InlineData("tolkien was john and thats all", "John Tolkien", true)]
    [InlineData("Arnold tolkIen Maria jOhn Lichtenstain", "JohN ToLkien", true)]
    [InlineData("4 is the best test for this", "this is test 4", true)]
    [InlineData("4 word is something this text test", "this is test 4", true)]
    [InlineData("4 word iS something thIs text TEST", "this is test 4", true)]
    [InlineData("Do you know Aragorn Legolas and Gandalf?", "Aragorn Legolas", true)]
    [InlineData("Aragorn STH Legolas", "Aragorn Legolas", true)]
    [InlineData("this is test 4", "this IS test 4", true)]
    [InlineData("this is test 4", "this is TEST 4", true)]
    [InlineData("this is test 4", "THIS is TEST 4", true)]
    [InlineData("TEXT Legolas SOME Aragorn end", "Aragorn Legolas", true)]
    [InlineData("TEXT some words legolaS SOME words here aragorN", "Aragorn Legolas", true)]
    [InlineData("is this test 4?", "this is test 4", true)]
    [InlineData("Legolas-Aragorn", "Aragorn Legolas", true)]
    [InlineData("Legolas, Aragorn, Gandalf", "Aragorn Legolas", true)]
    [InlineData("JohnTolkien", "John Tolkien", true)]
    [InlineData("johntolkien", "John Tolkien", true)]
    [InlineData("TolkienJohn", "John Tolkien", true)]
    [InlineData("tolkienjohn", "John Tolkien", true)]
    [InlineData("John Tolkien", "TolkienJohn", false)]
    [InlineData("Tolkian Johnson", "John Tolkien", false)]
    [InlineData("John Tolkianova", "John Tolkien", false)]
    [InlineData("John Some word here to be longer", "John Tolkien", false)]
    [InlineData("Legolas Aragon", "Aragorn Legolas", false)]
    [InlineData("Leolas Aragorn", "Aragorn Legolas", false)]
    [InlineData("sth totally different", "Aragorn Legolas", false)]
    [InlineData("is test 4", "this is test 4", false)]
    [InlineData("this is test ", "this is test 4", false)]
    [InlineData("this is text 4", "this is test 4", false)]
    [InlineData("this is est 4", "this is test 4", false)]
    [InlineData("ths s test 4", "ths is test 4", false)]
    [InlineData("this is 4", "this is test 4", false)]
    [InlineData("this is test 6", "this is test 4", false)]
    [InlineData("ths is test 4", "this is test 4", false)]
    public void GetQuestionByName_ExpectToFindIfShouldBeFoundIsTrue(string questionName, string searchString, bool shouldBeFound) {
        // Arrange
        QuestionFacade questionFacade = GetFacadeWithForbiddenDependencyCalls();

        var questions = new List<QuestionEntity>() {
            new() { Id = new Guid(), Text = questionName, Type = AnswerFormat.SingleChoice }
        };

        _questionRepositoryMock.Setup(rep => rep.GetByPredicate(It.IsAny<Expression<Func<QuestionEntity, bool>>>(), It.IsAny<string[]>()))
                           .Returns<Expression<Func<QuestionEntity, bool>>, string[]>((predicate, nav) => questions.Where(predicate.Compile()));

        // Act 
        IEnumerable<QuestionDetailModel> questionDetailModels = questionFacade.GetQuestionByName(searchString);

        // Assert
        Assert.NotNull(questionDetailModels);
        Assert.Equal(shouldBeFound? 1:0, questionDetailModels.Count());
    }


    private void SetUpMocksBase() {
        _uowMock.Setup(uow => uow.GetRepository<QuestionEntity>()).Returns(_questionRepositoryMock.Object);
        _uowMock.Setup(uow => uow.GetRepository<QuizEntity>()).Returns(_quizRepositoryMock.Object);
        
        _mapperMock.Setup(map => map.Map<QuestionEntity>(It.IsAny<QuestionDetailModel>()))
                   .Returns<QuestionDetailModel>(a => new QuestionEntity { Id = a.Id, Text = a.Text, Type = a.QuestionType });
        _mapperMock.Setup(map => map.Map<QuestionDetailModel>(It.IsAny<QuestionEntity>()))
                   .Returns<QuestionEntity>(a => new QuestionDetailModel {
                       Id = a.Id,
                       Text = a.Text
                   });

        _quizChecker.Setup(chck => chck.IsQuizEditable(It.IsAny<Guid>())).Returns<Guid>(_ => true);
    }

    private QuestionFacade GetFacadeWithForbiddenDependencyCalls() {
        SetUpMocksBase();
        var facade = new QuestionFacade(_uowFactoryMock.Object, _mapperMock.Object, _quizChecker.Object);
        return facade;
    }
}
