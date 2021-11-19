using System;
using System.Collections.Generic;
using System.Linq;
using AzureFunctionsREST.Domain.Models;
using AzureFunctionsREST.Domain.Utilities;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;

namespace AzureFunctionsREST.Infrastructure.Repositories
{
    public class WeatherForecastRepository : GenericRepository<WeatherForecast>
    {
        private static readonly string _collectionName = "Forecasts";

        private readonly ReporterRepository _reporterRepository;

        public WeatherForecastRepository(IConfiguration configuration, ReporterRepository reporterRepository)
                : base(_collectionName, configuration)
        {
            this._reporterRepository = reporterRepository;
        }

        public dynamic[] All(string[] populate)
        {
            IEnumerable<WeatherForecast> weatherForecasts = All();
            IEnumerable<dynamic> populatedWeatherForecasts = weatherForecasts.Select(forecast =>
             {
                 dynamic populatedWeatherForecast = ModelUtility.ConvertToExpandoObject(forecast);
                 if (Array.IndexOf(populate, "reporter") > -1)
                 {
                     populatedWeatherForecast.reporter = _reporterRepository.Get(new ObjectId(forecast.Reporter));
                 }
                 return populatedWeatherForecast;
             });
            return populatedWeatherForecasts.ToArray();
        }

        public dynamic Get(ObjectId id, string[] populate)
        {
            WeatherForecast weatherForecast = Get(id);
            dynamic populatedWeatherForecast = ModelUtility.ConvertToExpandoObject(weatherForecast);
            if (Array.IndexOf(populate, "reporter") > -1)
            {
                populatedWeatherForecast.reporter = _reporterRepository.Get(new ObjectId(weatherForecast.Reporter));
            }
            return populatedWeatherForecast;
        }

    }
}