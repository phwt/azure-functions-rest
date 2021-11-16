using System;

namespace AzureFunctionsREST.API.Models
{
    public class WeatherForecastRequest {

        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
    }
}