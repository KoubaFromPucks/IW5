using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quizzer.API.DAL.Migrator;

namespace Quizzer.API.DAL;
public class ApiDALInstaller {
    public void Install(IServiceCollection services, string connectionString) {
        services.AddDbContextFactory<QuizzerDbContext>(options => options.UseSqlServer(connectionString));
        services.AddSingleton<IDbMigrator, SqlDatabaseMigrator>();
    }
}