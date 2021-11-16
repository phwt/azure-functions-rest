using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using AzureFunctionsREST.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;

namespace AzureFunctionsREST.API
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
                                services.AddTransient<WeatherForecastRepository>(); // TODO: Specify repository interface
                            })
                            .ConfigureAppConfiguration(builder => builder.AddEnvironmentVariables())
                            .Build();

            host.Run();
        }
    }
}