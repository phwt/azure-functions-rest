using System;
using System.Linq;
using System.Threading.Tasks;
using AzureFunctionsREST.Domain.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AzureFunctionsREST.Domain.Services
{
    public class WeatherForecastService
    {
        private readonly IMongoCollection<WeatherForecast> _forecasts;

        public WeatherForecastService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MONGODB_CONNECTION_STRING"]);
            var database = client.GetDatabase(configuration["MONGODB_DBNAME"]);
            this._forecasts = database.GetCollection<WeatherForecast>(configuration["MONGODB_COLLECTION_NAME"]);
        }

        public WeatherForecast[] Get()
        {
            return _forecasts.Find(_ => true).ToEnumerable().ToArray();
        }

        public WeatherForecast Create(WeatherForecast weatherForecast)
        {
            _forecasts.InsertOne(weatherForecast);
            return weatherForecast;
        }
    }
}