namespace AzureFunctionsREST.Domain.Models
{
    public class Station : BaseMongoModel
    {
        public string Name { get; set; }
        public string Location { get; set; }
    }
}