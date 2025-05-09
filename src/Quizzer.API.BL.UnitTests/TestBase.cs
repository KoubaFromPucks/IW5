using System.Globalization;

namespace Quizzer.API.BL.UnitTests; 
public abstract class TestBase {
    protected Mock<IUnitOfWork> _uowMock = new(MockBehavior.Strict);
    protected Mock<IUnitOfWorkFactory> _uowFactoryMock = new(MockBehavior.Strict);
    protected Mock<IMapper> _mapperMock = new(MockBehavior.Strict);

    public TestBase() {
        _uowMock.Setup(uow => uow.Dispose());
        _uowMock.Setup(uow => uow.Commit());
        _uowFactoryMock.Setup(factory => factory.Create()).Returns(_uowMock.Object);
    }
    
    protected static DateTime ParseDateTime(string date, string format = "d.M.yyyy H:m:s") {
        DateTime parsedDateTime;

        if (DateTime.TryParseExact(date, format, null, DateTimeStyles.None, out parsedDateTime)) {
            return parsedDateTime;
        }

        throw new ArgumentException("Datetime string was not in correct format.");
    }
}
