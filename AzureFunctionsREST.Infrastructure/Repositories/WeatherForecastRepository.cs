using AzureFunctionsREST.Domain.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AzureFunctionsREST.Infrastructure.Repositories
{
    public class WeatherForecastRepository : GenericRepository<WeatherForecast>
    {
        public WeatherForecastRepository(IConfiguration configuration)
                : base(GetDatabaseCollection<WeatherForecast>(configuration["MONGODB_COLLECTION_NAME"],
                                                              configuration))
        { }

        public static IMongoCollection<T> GetDatabaseCollection<T>(string collectionName, IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MONGODB_CONNECTION_STRING"]);
            var database = client.GetDatabase(configuration["MONGODB_DBNAME"]);
            return database.GetCollection<T>(collectionName);
        }
    }
}