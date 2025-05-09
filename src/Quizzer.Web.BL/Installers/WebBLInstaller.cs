using Microsoft.Extensions.DependencyInjection;
using Quizzer.Web.BL.Facades;

namespace Quizzer.Web.BL.Installers; 
public class WebBLInstaller {
    public void Install(IServiceCollection services, string apiBaseUrl) {
        services.AddTransient<IAnswerApiClient, AnswerApiClient>(provider =>
        {
            HttpClient client = CreateApiHttpClient(apiBaseUrl);
            return new AnswerApiClient(apiBaseUrl, client);
        });

        services.AddTransient<IQuestionApiClient, QuestionApiClient>(provider =>
        {
            HttpClient client = CreateApiHttpClient(apiBaseUrl);
            return new QuestionApiClient(apiBaseUrl, client);
        });

        services.AddTransient<IQuizApiClient, QuizApiClient>(provider =>
        {
            HttpClient client = CreateApiHttpClient(apiBaseUrl);
            return new QuizApiClient(apiBaseUrl, client);
        });

        services.AddTransient<IUserApiClient, UserApiClient>(provider =>
        {
            HttpClient client = CreateApiHttpClient(apiBaseUrl);
            return new UserApiClient(apiBaseUrl, client);
        });

        services.Scan(selector =>
            selector.FromAssemblyOf<WebBLInstaller>()
                .AddClasses(classes => classes.AssignableTo<IAppFacade>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime());
    }

    public HttpClient CreateApiHttpClient(string apiBaseUrl) {
        var client = new HttpClient() { BaseAddress = new Uri(apiBaseUrl) };
        client.BaseAddress = new Uri(apiBaseUrl);
        return client;
    }
}
