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
                                // TODO: Specify repository interface
                                services.AddTransient<WeatherForecastRepository>(); 
                                services.AddTransient<ReporterRepository>(); 
                            })
                            .ConfigureAppConfiguration(builder => builder.AddEnvironmentVariables())
                            .Build();

            host.Run();
        }
    }
}