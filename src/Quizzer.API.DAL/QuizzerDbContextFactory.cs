using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Quizzer.API.DAL; 
public class QuizzerDbContextFactory : IDesignTimeDbContextFactory<QuizzerDbContext> {
    public QuizzerDbContextFactory() {
           
    }
    public QuizzerDbContext CreateDbContext(string[] args) {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddUserSecrets<QuizzerDbContext>(optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<QuizzerDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        return new QuizzerDbContext(optionsBuilder.Options);
    }
}
