using Microsoft.EntityFrameworkCore;

namespace Quizzer.API.DAL.Migrator;
public class SqlDatabaseMigrator : IDbMigrator {
    private readonly IDbContextFactory<QuizzerDbContext> _dbContextFactory;

    public SqlDatabaseMigrator(IDbContextFactory<QuizzerDbContext> dbContextFactory) {
        _dbContextFactory = dbContextFactory;
    }

    public void Migrate() {
        using QuizzerDbContext dbContext = _dbContextFactory.CreateDbContext();

        dbContext.Database.Migrate();
    }
}
