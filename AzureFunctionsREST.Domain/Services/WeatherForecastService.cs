using System.Collections.Generic;
using AzureFunctionsREST.Domain.Interfaces;
using AzureFunctionsREST.Domain.Models;
using MongoDB.Driver;

namespace AzureFunctionsREST.Domain.Services
{
    public class WeatherForecastService
    {
        IWeatherForecastRepository _weatherForecastRepository;
        public WeatherForecastService(IWeatherForecastRepository weatherForecastRepository)
        {
            _weatherForecastRepository = weatherForecastRepository;
        }

#nullable enable
        public IEnumerable<WeatherForecast> Filter(string? reporterId, string? stationId)
        {
            var filter = Builders<WeatherForecast>.Filter.And(
                reporterId != null ? Builders<WeatherForecast>.Filter.Eq(forecast => forecast.Reporter, reporterId) : Builders<WeatherForecast>.Filter.Empty,
                stationId != null ? Builders<WeatherForecast>.Filter.Eq(forecast => forecast.Station, stationId) : Builders<WeatherForecast>.Filter.Empty
            );
            return _weatherForecastRepository.Find(filter);
        }
    }
}