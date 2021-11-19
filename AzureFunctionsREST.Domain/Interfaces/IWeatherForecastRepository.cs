using AzureFunctionsREST.Domain.Models;
using MongoDB.Bson;

namespace AzureFunctionsREST.Domain.Interfaces
{
    public interface IWeatherForecastRepository : IRepository<WeatherForecast>
    {
        dynamic[] All(string[] populate);
        dynamic Get(ObjectId id, string[] populate);
    }
}