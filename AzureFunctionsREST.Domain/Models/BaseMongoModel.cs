using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AzureFunctionsREST.Domain.Models
{
    public abstract class BaseMongoModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}