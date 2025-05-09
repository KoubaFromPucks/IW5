using Microsoft.EntityFrameworkCore;
using Quizzer.API.DAL.Seeds;

namespace Quizzer.API.DAL.Tests; 
public class QuizzerDbContextTestingFactory : IDbContextFactory<QuizzerDbContext> {
    private readonly string _databaseName;
    private readonly Storage _storage;

    public QuizzerDbContextTestingFactory (string databaseName, bool seedTestingData = false) {
        _databaseName = databaseName;
        _storage = new Storage(seedTestingData);
    }

    public QuizzerDbContext CreateDbContext() {
        DbContextOptionsBuilder<QuizzerDbContext> builder = new();

        builder.UseSqlite($"Data Source={_databaseName};Cache=Shared");

        return new QuizzerTestingDbContext(builder.Options, _storage);
    }
}
