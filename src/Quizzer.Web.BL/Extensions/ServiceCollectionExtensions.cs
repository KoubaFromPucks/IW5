using Microsoft.Extensions.DependencyInjection;
using Quizzer.Web.BL.Installers;

namespace Quizzer.Web.BL.Extensions; 
public static class ServiceCollectionExtensions {
    public static void AddWebBlInstaller<TInstaller>(this IServiceCollection serviceCollection, string apiBaseUrl)
        where TInstaller : WebBLInstaller, new() {
        TInstaller installer = new();

        installer.Install(serviceCollection, apiBaseUrl);
    }
}
