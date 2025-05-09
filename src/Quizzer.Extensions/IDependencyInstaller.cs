using Microsoft.Extensions.DependencyInjection;

namespace Quizzer.Extensions; 
public interface IDependencyInstaller {
    void Install(IServiceCollection services);
}