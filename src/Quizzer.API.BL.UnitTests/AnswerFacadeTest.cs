using Quizzer.Common.Models;
using System.Linq.Expressions;

namespace Quizzer.API.BL.UnitTests; 
public class AnswerFacadeTest : TestBase {
    private Mock<IRepository<AnswerEntity>> _answerRepositoryMock = new(MockBehavior.Strict);
    private Mock<IRepository<QuestionEntity>> _questionRepositoryMock = new(MockBehavior.Strict);
    private Mock<IQuizChecker> _quizChecker = new(MockBehavior.Strict);

    [Fact]
    public void GetAllAnswers_Calls_Correct_Methods() {
        // Arrange
        AnswerFacade answerFacade = GetFacadeWithForbiddenDependencyCalls();

        _answerRepositoryMock.Setup(rep => rep.GetAll()).Returns(new List<AnswerEntity>());
        _mapperMock.Setup(mapper => mapper.Map<AnswerDetailModel>(It.IsAny<AnswerEntity>()))
                   .Returns(new AnswerDetailModel { Id = Guid.NewGuid(), Text = "empty", QuestionId = Guid.NewGuid(), IsCorrect = false });

        // Act
        IEnumerable<AnswerDetailModel> models = answerFacade.GetAllAnswers();

        // Assert
        _uowFactoryMock.Verify(factory => factory.Create(), Times.Once);
        _uowMock.Verify(uow => uow.GetRepository<AnswerEntity>(), Times.Once);
        _answerRepositoryMock.Verify(rep => rep.GetAll());
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-80)]
    [InlineData(-11981)]
    [InlineData(-110)]
    [InlineData(-2147483648)]
    [InlineData(-2)]
    public void Save_Answer_With_Bad_Order_ThrowsException(int badOrder) {
        // Arrange 
        var answerDetailModel = new AnswerDetailModel { 
            Id = Guid.Empty, 
            Text = "", 
            Order = badOrder, 
            IsCorrect = false, 
            QuestionId = Guid.NewGuid()
        };
        AnswerFacade answerFacade = GetFacadeWithForbiddenDependencyCalls();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => answerFacade.Save(answerDetailModel, Guid.NewGuid()));
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(null, "")]
    [InlineData(null, "              ")]
    [InlineData(null, "\t")]
    [InlineData("", null)]
    [InlineData("", "")]
    [InlineData("", "       ")]
    [InlineData("", "\t")]
    [InlineData("         ", null)]
    [InlineData("         ", "")]
    [InlineData("         ", "     ")]
    [InlineData("         ", "\t")]
    [InlineData("\t", null)]
    [InlineData("\t", "")]
    [InlineData("\t", "    ")]
    [InlineData("\t", "\t")]
    public void Save_Answer_With_Bad_Text_Or_Url_ThrowsException(string text, string url) {
        // Arrange 
        var answerDetailModel = new AnswerDetailModel { 
            Id = Guid.Empty, 
            Text = text, 
            Order = 5, 
            PictureUrl = url,
            IsCorrect = false,
            QuestionId = Guid.NewGuid()
        };
        AnswerFacade answerFacade = GetFacadeWithForbiddenDependencyCalls();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => answerFacade.Save(answerDetailModel, Guid.NewGuid()));
    }

    [Theory]
    [InlineData(null, "url2")]
    [InlineData("", "url1")]
    [InlineData("         ", "3url")]
    [InlineData("\t", "url6")]
    [InlineData("Some name", null)]
    [InlineData("I am not empty", "")]
    [InlineData("got this text?", "            ")]
    [InlineData("feel free?", "\t")]
    [InlineData(null, "url2", false)]
    [InlineData("", "url1", false)]
    [InlineData("         ", "3url", false)]
    [InlineData("\t", "url6", false)]
    [InlineData("Some name", null, false)]
    [InlineData("I am not empty", "", false)]
    [InlineData("got this text?", "            ", false)]
    [InlineData("feel free?", "\t", false)]
    public void Save_Answer_With_Bad_Text_Or_Url_Expected_Call_Save(string text, string url, bool isNew = true) {
        // Arrange 
        var answerDetailModel = new AnswerDetailModel { 
            Id = Guid.NewGuid(), 
            Text = text, 
            Order = 5, 
            PictureUrl = url,
            IsCorrect = false,
            QuestionId = Guid.NewGuid()
        };
        var answerEntity = new AnswerEntity { Id = Guid.NewGuid(), IsCorrect = true, Text = "sth" };
        AnswerFacade answerFacade = GetFacadeWithForbiddenDependencyCalls();

        _mapperMock.Setup(mapper => mapper.Map<AnswerEntity>(It.IsAny<AnswerDetailModel>()))
                   .Returns(new AnswerEntity { Id = Guid.NewGuid(), Text = "empty", IsCorrect = false });

        _answerRepositoryMock.Setup(rep => rep.Exists(It.IsAny<Guid>())).Returns(true);
        _answerRepositoryMock.Setup(rep => rep.Update(It.IsAny<AnswerEntity>())).Returns(Guid.NewGuid());
        _answerRepositoryMock.Setup(rep => rep.GetById(It.IsAny<Guid>())).Returns(isNew? It.IsAny<AnswerEntity>() : answerEntity);
        _questionRepositoryMock.Setup(rep => rep.GetById(It.IsAny<Guid>(), nameof(QuestionEntity.Quiz)))
                              .Returns(new QuestionEntity { 
                                  Id = Guid.NewGuid(), Text = "sth", Type = answerDetailModel.Type,
                                  Quiz = new QuizEntity {
                                      Id = Guid.NewGuid(), 
                                      Name = "name", 
                                      StartTime = DateTime.Now.AddDays(10), 
                                      EndTime = DateTime.Now.AddDays(15)
                                  }
                              });

        // Act
        answerFacade.Save(answerDetailModel, Guid.NewGuid());

        // Assert
        _mapperMock.Verify(map => map.Map<AnswerEntity>(It.IsAny<AnswerDetailModel>()), Times.Once);
        _answerRepositoryMock.Verify(rep => rep.Exists(It.IsAny<Guid>()), Times.Once);
        _answerRepositoryMock.Verify(rep => rep.Update(It.IsAny<AnswerEntity>()), Times.Once);
        _uowMock.Verify(uow => uow.Commit(), Times.Once);
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
    public void GetByText_ExpectSelected(string answerText, string searchString, bool shouldBeFound) {
        // Arrange
        AnswerFacade answerFacade = GetFacadeWithForbiddenDependencyCalls();

        var answers = new List<AnswerEntity>() {
            new() {Id = new Guid(), IsCorrect = false, Text = answerText}
        };

        _answerRepositoryMock.Setup(rep => rep.GetByPredicate(It.IsAny<Expression<Func<AnswerEntity, bool>>>(), It.IsAny<string[]>()))
                           .Returns<Expression<Func<AnswerEntity, bool>>, string[]>((predicate, nav) => answers.Where(predicate.Compile()));

        // Act 
        IEnumerable<AnswerDetailModel> answerDetailModels = answerFacade.GetAnswerByText(searchString);

        // Assert
        Assert.NotNull(answerDetailModels);
        Assert.Equal(shouldBeFound? 1:0, answerDetailModels.Count());
    }

    private void SetUpMocksBase() {
        _uowMock.Setup(uow => uow.GetRepository<AnswerEntity>()).Returns(_answerRepositoryMock.Object);
        _uowMock.Setup(uow => uow.GetRepository<QuestionEntity>()).Returns(_questionRepositoryMock.Object);

        _mapperMock.Setup(map => map.Map<AnswerDetailModel>(It.IsAny<AnswerEntity>()))
                   .Returns<AnswerEntity>(a => new AnswerDetailModel {
                       Id = a.Id,
                       Text = a.Text,
                       IsCorrect = false,
                       QuestionId = Guid.NewGuid()
                   });

        _quizChecker.Setup(chck => chck.IsQuizEditable(It.IsAny<Guid>())).Returns<Guid>(_ => true);

        _questionRepositoryMock.Setup(rep => rep.GetById(It.IsAny<Guid>(), It.IsAny<string[]>()))
            .Returns<Guid, string[]>((id, props) => new QuestionEntity {
                Id = Guid.Empty,
                Text = "",
                Type = AnswerFormat.SingleChoice,
            });
    }

    private AnswerFacade GetFacadeWithForbiddenDependencyCalls() {
        SetUpMocksBase();
        var facade = new AnswerFacade(_uowFactoryMock.Object, _mapperMock.Object, _quizChecker.Object);
        return facade;
    }
}