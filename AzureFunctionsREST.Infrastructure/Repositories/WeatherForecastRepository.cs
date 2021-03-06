using AzureFunctionsREST.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace AzureFunctionsREST.Infrastructure.Repositories
{
    public class WeatherForecastRepository : GenericRepository<WeatherForecast>
    {
        private static readonly string _collectionName = "Forecasts";
        public WeatherForecastRepository(IConfiguration configuration)
                : base(_collectionName, configuration)
        { }

    }
}