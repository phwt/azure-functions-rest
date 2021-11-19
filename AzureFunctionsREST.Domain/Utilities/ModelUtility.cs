using System.Collections.Generic;
using System.Dynamic;

namespace AzureFunctionsREST.Domain.Utilities
{
    public static class ModelUtility
    {
        public static ExpandoObject ConvertToExpandoObject(object inputObject)
        {

            dynamic expandoObject = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)expandoObject;
            foreach (var property in inputObject.GetType().GetProperties())
            {
                dictionary.Add(property.Name, property.GetValue(inputObject));
            }
            return dictionary as ExpandoObject;
        }
    }
}