namespace Quizzer.API.DAL.UnitOfWork; 
public interface IUnitOfWorkFactory {
    IUnitOfWork Create();
}
