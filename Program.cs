using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Configuration;
using azure_functions_dotnet_rest_api.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;

namespace azure_functions_dotnet_rest_api
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                            .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
                            .ConfigureOpenApi()
                            .ConfigureServices(services =>
                            {
                                services.AddTransient<WeatherForecastService>();
                            })
                            .Build();

            host.Run();
        }
    }
}