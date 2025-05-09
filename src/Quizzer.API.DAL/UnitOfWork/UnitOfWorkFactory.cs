using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Quizzer.API.DAL.UnitOfWork; 
public class UnitOfWorkFactory : IUnitOfWorkFactory {
    private readonly IDbContextFactory<QuizzerDbContext> _dbContextFactory;

    public UnitOfWorkFactory(IDbContextFactory<QuizzerDbContext> dbContextFactory) =>
        _dbContextFactory = dbContextFactory;

    public IUnitOfWork Create() => new UnitOfWork(_dbContextFactory.CreateDbContext());
}