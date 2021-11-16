using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Configuration;
using AzureFunctionsREST.API.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using System;

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
                                services.AddTransient<WeatherForecastService>();
                            })
                            .ConfigureAppConfiguration(builder => builder.AddEnvironmentVariables())
                            .Build();

            host.Run();
        }
    }
}