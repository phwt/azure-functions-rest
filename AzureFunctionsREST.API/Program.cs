using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using AzureFunctionsREST.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using AzureFunctionsREST.Domain.Interfaces;
using AzureFunctionsREST.Domain.Services;

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
                                services.AddTransient<IWeatherForecastRepository, WeatherForecastRepository>();
                                services.AddTransient<IReporterRepository, ReporterRepository>();
                                services.AddTransient<IStationRepository, StationRepository>();

                                services.AddTransient<WeatherForecastService>();
                            })
                            .ConfigureAppConfiguration(builder => builder.AddEnvironmentVariables())
                            .Build();

            host.Run();
        }
    }
}