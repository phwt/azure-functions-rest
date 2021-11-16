using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;

namespace AzureFunctionsREST.API.Functions {
    public abstract class BaseFunction {

        public T DeserializeBody<T>(Stream body)
        {
            using (StreamReader reader = new StreamReader(body))
            {
                var bodyString = reader.ReadToEnd();
                return JsonSerializer.Deserialize<T>(bodyString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        public IReadOnlyDictionary<string, object> ExtractBindingData(FunctionContext functionContext) => functionContext.BindingContext.BindingData;
    }
}