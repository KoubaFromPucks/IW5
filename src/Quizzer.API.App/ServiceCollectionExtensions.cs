using Quizzer.API.BL;
using Quizzer.API.DAL;

namespace Quizzer.API; 
public static class ServiceCollectionExtensions {
    public static void AddDALInstaller<TInstaller>(this IServiceCollection services, string connectionString)
       where TInstaller : ApiDALInstaller, new() {
        TInstaller installer = new();

        installer.Install(services, connectionString);
    }

    public static void AddBLInstaller<TInstaller>(this IServiceCollection services)
       where TInstaller : ApiBLInstaller, new() {
        TInstaller installer = new();

        installer.Install(services);
    }
}
