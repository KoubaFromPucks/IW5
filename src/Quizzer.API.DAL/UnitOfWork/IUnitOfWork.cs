using Quizzer.API.DAL.Entities.Interface;
using Quizzer.API.DAL.Repository.Interfaces;

namespace Quizzer.API.DAL.UnitOfWork; 
public interface IUnitOfWork : IDisposable {
    /// <summary>
    /// Creates a new instance of repository with included database context
    /// </summary>
    /// <typeparam name="TEntity">Entity type capable of holding database entity data</typeparam>
    /// <typeparam name="TEntityMapper">Entity mapper for scalar properties between two entities instances</typeparam>
    /// <returns></returns>
    IRepository<TEntity> GetRepository<TEntity>()
        where TEntity : class, IEntity;

    /// <summary>
    /// Makes changes on entities in database
    /// </summary>
    void Commit();

    /// <summary>
    /// Makes changes on entities in database (asynchronous)
    /// </summary>
    Task CommitAsync();
}
