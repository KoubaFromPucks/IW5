using Microsoft.EntityFrameworkCore;
using Quizzer.API.DAL.Repository;
using Quizzer.API.DAL.Repository.Interfaces;

namespace Quizzer.API.DAL.UnitOfWork; 
public sealed class UnitOfWork : IUnitOfWork {
    private readonly DbContext _dbContext;
    /// <summary>
    /// Initializes a new instance of <see cref="UnitOfWork"/> class
    /// </summary>
    /// <param name="dbContext">Provided database context</param>
    /// <exception cref="ArgumentNullException">Db context is null</exception>
    public UnitOfWork(DbContext dbContext) =>
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task CommitAsync() => await _dbContext.SaveChangesAsync();

    public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync();

    IRepository<TEntity> IUnitOfWork.GetRepository<TEntity>() => new Repository<TEntity>(_dbContext);

    public void Commit() => _dbContext.SaveChanges();

    public void Dispose() => _dbContext.Dispose();
}
