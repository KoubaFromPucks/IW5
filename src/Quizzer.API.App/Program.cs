using AutoMapper;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using Quizzer.API.BL;
using Quizzer.API.DAL;
using Quizzer.API.DAL.Entities;
using Quizzer.API.DAL.Migrator;

namespace Quizzer.API;
public class Program {
    public static void Main(string[] args) {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        ConfigureDependencies(builder.Services, builder.Configuration);

        ConfigureAutoMapper(builder.Services);

        ConfigureCors(builder.Services);
        WebApplication app = builder.Build();

        app.Services.GetRequiredService<IDbMigrator>().Migrate();

        ValidateAutoMapperConfiguration(app.Services);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseCors();

        app.MapControllers();

        app.Run();
    }

    private static void ConfigureDependencies(IServiceCollection services, IConfigurationBuilder configuration) {
        IConfigurationRoot sconfiguration = configuration
            .AddUserSecrets<QuizzerDbContext>(optional: true).Build();
        string? connectionString = sconfiguration.GetConnectionString("DefaultConnection");

        services.AddDALInstaller<ApiDALInstaller>(connectionString ?? string.Empty);
        services.AddBLInstaller<ApiBLInstaller>();
    }

    private static void ConfigureAutoMapper(IServiceCollection serviceCollection) {
        //source https://github.com/nesfit/IW5
        serviceCollection.AddAutoMapper(configuration => {
            // This is a temporary fix - should be able to remove this when version 11.0.2 comes out
            // More information here: https://github.com/AutoMapper/AutoMapper/issues/3988
            configuration.Internal().MethodMappingEnabled = false;
        }, typeof(EntityBase), typeof(ApiBLInstaller));
    }

    private static void ValidateAutoMapperConfiguration(IServiceProvider serviceProvider) {
        //source https://github.com/nesfit/IW5
        IMapper mapper = serviceProvider.GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    private static void ConfigureCors(IServiceCollection serviceCollection) {
        serviceCollection.AddCors(options => {
            options.AddDefaultPolicy(o =>
                o.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });
    }
}