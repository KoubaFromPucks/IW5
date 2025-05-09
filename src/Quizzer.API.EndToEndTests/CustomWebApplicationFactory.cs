using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Quizzer.API.DAL;
using Quizzer.API.DAL.Seeds;


namespace Quizzer.API.IntegrationTests; 
public class CustomWebApplicationFactory<TProgram>
: WebApplicationFactory<TProgram> where TProgram : class {
    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.ConfigureServices(services => {

            ServiceDescriptor? dbContextOptionsDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<QuizzerDbContext>));

            services.Remove(dbContextOptionsDescriptor);

            ServiceDescriptor? dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(QuizzerDbContext));

            services.Remove(dbContextDescriptor);

            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddUserSecrets<QuizzerDbContext>(optional: true)
            .Build();

            var connectionString = configuration.GetConnectionString("TestingDatabase");

            services.AddDbContextFactory<QuizzerDbContext>(
                options => options.UseSqlServer(connectionString)
            );

        });

        builder.UseEnvironment("Development");
    }
}
