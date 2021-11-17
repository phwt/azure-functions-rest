using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AzureFunctionsREST.Domain.Exceptions;
using FluentValidation;
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

        protected TDeserializeType DeserializeBody<TDeserializeType, TValidator>(Stream body)
            where TValidator : AbstractValidator<TDeserializeType>
        {
            TDeserializeType deserializedBody = DeserializeBody<TDeserializeType>(body);

            TValidator validator = (TValidator)Activator.CreateInstance(typeof(TValidator), new object[] { });
            validator.ValidateAndThrow(deserializedBody);

            return deserializedBody;
        }

        protected IReadOnlyDictionary<string, object> ExtractBindingData(FunctionContext functionContext) => functionContext.BindingContext.BindingData;

        private HttpResponseData BuildErrorResponse(HttpRequestData requestData, Exception exception, HttpStatusCode statusCode)
        {
            var response = requestData.CreateResponse(statusCode);
            response.WriteString(exception.Message);
            return response;
        }

        protected async Task<HttpResponseData> RequestWrapper(HttpRequestData requestData, Func<HttpResponseData, Task<HttpResponseData>> requestHandler)
        {
            try
            {
                var response = requestData.CreateResponse(HttpStatusCode.OK);
                return await requestHandler(response);
            }
            catch (DocumentNotFoundException exception)
            {
                return BuildErrorResponse(requestData, exception, HttpStatusCode.NotFound);
            }
            catch (FormatException exception)
            {
                return BuildErrorResponse(requestData, exception, HttpStatusCode.BadRequest);
            }
            catch (Exception exception) when (exception is JsonException || exception is ValidationException)
            {
                return BuildErrorResponse(requestData, exception, HttpStatusCode.UnprocessableEntity);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}