namespace AzureFunctionsREST.Domain.Models
{
    public class Reporter : BaseMongoModel
    {
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
    }
}