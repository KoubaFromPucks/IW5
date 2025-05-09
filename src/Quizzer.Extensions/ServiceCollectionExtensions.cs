using Microsoft.Extensions.DependencyInjection;

namespace Quizzer.Extensions; 
public static class ServiceCollectionExtensions {
    public static void AddInstaller<TInstaller>(this IServiceCollection services) 
        where TInstaller : IDependencyInstaller, new() {

        TInstaller installer = new();
        installer.Install(services);
    }
}
