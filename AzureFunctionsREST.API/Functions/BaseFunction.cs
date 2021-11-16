using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AzureFunctionsREST.Domain.Exceptions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace AzureFunctionsREST.API.Functions
{
    public abstract class BaseFunction
    {

        protected T DeserializeBody<T>(Stream body)
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

        protected IReadOnlyDictionary<string, object> ExtractBindingData(FunctionContext functionContext) => functionContext.BindingContext.BindingData;

        protected async Task<HttpResponseData> RequestWrapper(HttpRequestData requestData, Func<HttpResponseData, Task<HttpResponseData>> requestHandler) {
            try
            {
                var response = requestData.CreateResponse(HttpStatusCode.OK);
                return await requestHandler(response);
            }
            catch (DocumentNotFoundException)
            {
                return requestData.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}