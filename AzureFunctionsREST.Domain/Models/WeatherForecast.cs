using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AzureFunctionsREST.Domain.Models
{
    public class WeatherForecast: BaseMongoModel
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Reporter { get; set; }
    }
}
