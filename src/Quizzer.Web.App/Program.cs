using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Quizzer.Web.BL.Installers;
using Quizzer.Web.BL.Extensions;

namespace Quizzer.Web.App; 
public class Program {
    public static async Task Main(string[] args) {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        var apiBaseUrl = builder.Configuration.GetValue<string>("ApiBaseUrl");


#if DEBUG
        apiBaseUrl = "https://localhost:7216/";
#endif
        builder.Services.AddWebBlInstaller<WebBLInstaller>(apiBaseUrl ?? string.Empty);
        builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        await builder.Build().RunAsync();
    }
}