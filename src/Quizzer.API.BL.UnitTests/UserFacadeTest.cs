using Quizzer.Common.Models;
using System.Linq.Expressions;

namespace Quizzer.API.BL.UnitTests; 
public class UserFacadeTest : TestBase {
    private Mock<IRepository<UserEntity>> _userRepositoryMock = new(MockBehavior.Strict);
    private const string NESTED_NAME = "name";

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
    public void Save_Bad_Name_ThrowsException(string name) {
        // Arrange
        var userDetailModel = new UserDetailModel {Id = Guid.NewGuid(), Name = name };
        UserFacade userFacade = GetFacadeWithForbiddenDependencyCalls();
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => userFacade.Save(userDetailModel));
    }

    [Theory]
    [InlineData("    " + NESTED_NAME + "     ", true)]
    [InlineData("\t" + NESTED_NAME + "     ", true)]
    [InlineData("\n   \t" + NESTED_NAME + "\0\v\f", true)]
    [InlineData("" + NESTED_NAME + "", true)]
    [InlineData("\0\a\r\b" + NESTED_NAME + "\0\a\r\b", true)]
    [InlineData("    " + NESTED_NAME + "     ", false)]
    [InlineData("\t" + NESTED_NAME + "     ", false)]
    [InlineData("\n   \t" + NESTED_NAME + "\0\v\f", false)]
    [InlineData("" + NESTED_NAME + "", false)]
    [InlineData("\0\a\r\b" + NESTED_NAME + "\0\a\r\b", false)]
    public void Save_OkName_Expect_Call_Update_Or_Insert_WithTrimedName(string name, bool exists) {
        // Arrange
        string savedName =  "";
        var userDetailModel = new UserDetailModel { Id = Guid.NewGuid(), Name = name };
        UserFacade userFacade = GetFacadeWithForbiddenDependencyCalls();

        _mapperMock.Setup(map => map.Map<UserEntity>(It.IsAny<UserDetailModel>()))
                   .Returns<UserDetailModel>(model => new UserEntity { Id = model.Id, Name = model.Name ?? ""});
        _userRepositoryMock.Setup(rep => rep.Exists(userDetailModel.Id)).Returns(exists);
        _userRepositoryMock.Setup(rep => rep.Update(It.IsAny<UserEntity>()))
            .Callback((UserEntity ent) => savedName = ent.Name)
            .Returns(userDetailModel.Id);
        _userRepositoryMock.Setup(rep => rep.Insert(It.IsAny<UserEntity>()))
            .Callback((UserEntity ent) => savedName = ent.Name)
            .Returns(userDetailModel.Id);

        // Act
        userFacade.Save(userDetailModel);

        // Assert
        if (exists) {
            _userRepositoryMock.Verify(rep => rep.Update(It.IsAny<UserEntity>()), Times.Once());
        } else {
            _userRepositoryMock.Verify(rep => rep.Insert(It.IsAny<UserEntity>()), Times.Once());
        }
        
        _uowMock.Verify(uow => uow.Commit(), Times.Once());
        Assert.Equal(NESTED_NAME, savedName);
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
    public void GetUserByName_ExpecteSelected(string userName, string searchString, bool shouldBeFound) {
        // Arrange
        UserFacade userFacade = GetFacadeWithForbiddenDependencyCalls();

        var users = new List<UserEntity>() {
            new UserEntity { Id = new Guid(), Name = userName}
        };

        _userRepositoryMock.Setup(rep => rep.GetByPredicate(It.IsAny<Expression<Func<UserEntity, bool>>>(), It.IsAny<string[]>()))
                           .Returns<Expression<Func<UserEntity, bool>>, string[]>((predicate, nav) => users.Where(predicate.Compile()));

        // Act 
        IEnumerable<UserListModel> userListModels = userFacade.GetByName(searchString, false);

        // Assert
        Assert.NotNull(userListModels);
        Assert.Equal(shouldBeFound? 1:0, userListModels.Count());
    }

    private void SetUpMocksBase() {
        _uowMock.Setup(uow => uow.GetRepository<UserEntity>()).Returns(_userRepositoryMock.Object);
        _mapperMock.Setup(map => map.Map<UserListModel>(It.IsAny<UserEntity>()))
                   .Returns<UserEntity>(a => new UserListModel {
                       Id = a.Id,
                       Name = a.Name
                   });
    }

    private UserFacade GetFacadeWithForbiddenDependencyCalls() {
        SetUpMocksBase();
        var facade = new UserFacade(_uowFactoryMock.Object, _mapperMock.Object);
        return facade;
    }
}
