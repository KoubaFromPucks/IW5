using Microsoft.Extensions.DependencyInjection;
using Quizzer.API.BL.Facades;
using Quizzer.API.DAL.UnitOfWork;
using Quizzer.Extensions;

namespace Quizzer.API.BL; 
public class ApiBLInstaller : IDependencyInstaller {
    public void Install(IServiceCollection services) {
        services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();

        services.Scan(selector => selector
        .FromAssembliesOf(typeof(IFacade<,>))
        .AddClasses(filter => filter.AssignableTo(typeof(IFacade<,>)))
        .AsMatchingInterface()
        .AsSelfWithInterfaces()
        .WithScopedLifetime());
    }
}
