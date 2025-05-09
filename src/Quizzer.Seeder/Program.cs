using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quizzer.API.DAL;
using Quizzer.API.DAL.Seeds;

namespace Quizzer.Seeder;
public class Program {
    public static void Main(string[] args) {
        IConfigurationRoot conWithSecrets =
            new ConfigurationBuilder().AddUserSecrets<QuizzerDbContext>(optional: true).Build();

        var optionsBuilder = new DbContextOptionsBuilder<QuizzerDbContext>();
        optionsBuilder.UseSqlServer(conWithSecrets.GetConnectionString("DefaultConnection"));

        var dbContext = new QuizzerDbContext(optionsBuilder.Options);

        var seeder = new QuizzerSeeder(dbContext);
        seeder.ClearDatabase();
        var storage = new Storage(true);
        seeder.SeedDatabase(storage);
    }
}
