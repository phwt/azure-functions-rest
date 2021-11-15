using azure_functions_dotnet_rest_api.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(azure_functions_dotnet_rest_api.Startup))]
namespace azure_functions_dotnet_rest_api
{
    public class Startup : FunctionsStartup {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<WeatherForecastService>();
        }
    }
}