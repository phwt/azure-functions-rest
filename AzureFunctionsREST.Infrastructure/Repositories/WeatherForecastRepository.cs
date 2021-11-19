using System;
using System.Collections.Generic;
using System.Linq;
using AzureFunctionsREST.Domain.Interfaces;
using AzureFunctionsREST.Domain.Models;
using AzureFunctionsREST.Domain.Utilities;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;

namespace AzureFunctionsREST.Infrastructure.Repositories
{
    public class WeatherForecastRepository : GenericRepository<WeatherForecast>, IWeatherForecastRepository 
    {
        private static readonly string _collectionName = "Forecasts";

        private readonly IReporterRepository _reporterRepository;

        public WeatherForecastRepository(IConfiguration configuration, IReporterRepository reporterRepository)
                : base(_collectionName, configuration)
        {
            this._reporterRepository = reporterRepository;
        }

        private dynamic PopulateFields(WeatherForecast forecast, string[] populate)
        {
            dynamic populatedWeatherForecast = ModelUtility.ConvertToExpandoObject(forecast);
            if (Array.IndexOf(populate, "reporter") > -1)
            {
                populatedWeatherForecast.reporter = _reporterRepository.Get(new ObjectId(forecast.Reporter));
            }
            return populatedWeatherForecast;
        }

        public dynamic[] All(string[] populate)
        {
            IEnumerable<WeatherForecast> weatherForecasts = All();
            IEnumerable<dynamic> populatedWeatherForecasts = weatherForecasts.Select(forecast =>
             {
                 return PopulateFields(forecast, populate);
             });
            return populatedWeatherForecasts.ToArray();
        }

        public dynamic Get(ObjectId id, string[] populate)
        {
            WeatherForecast weatherForecast = Get(id);
            return PopulateFields(weatherForecast, populate);
        }

    }
}