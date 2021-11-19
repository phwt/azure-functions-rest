using System.Net;
using System.Threading.Tasks;
using AzureFunctionsREST.API.Models;
using AzureFunctionsREST.API.Validators;
using AzureFunctionsREST.Domain.Models;
using AzureFunctionsREST.Infrastructure.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;

namespace AzureFunctionsREST.API.Functions
{
    public class WeatherForecastFunction : BaseFunction
    {
        private readonly WeatherForecastRepository _weatherForecastRepository;

        public WeatherForecastFunction(WeatherForecastRepository weatherForecastRepository)
        {
            this._weatherForecastRepository = weatherForecastRepository;
        }

        [Function("WeatherForecastList")]
        [OpenApiOperation(tags: new[] { "forecast" }, Summary = "Retrieve all weather forecasts")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "populate", In = ParameterLocation.Query, Type = typeof(string[]))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WeatherForecast[]),
            Description = "All weather forecasts")]
        public async Task<HttpResponseData> List([HttpTrigger(AuthorizationLevel.Function, "get", Route = "weather")] HttpRequestData req,
                                                 FunctionContext executionContext)
        {
            return await RequestWrapper(req, async response =>
            {
                object populateObject;
                bool havePopulate = ExtractBindingData(executionContext).TryGetValue("populate", out populateObject);

                if (havePopulate)
                {
                    var result = this._weatherForecastRepository.All(populateObject.ToString().Split(","));
                    await response.WriteAsJsonAsync((object[])result);
                }
                else
                {
                    var result = this._weatherForecastRepository.All();
                    await response.WriteAsJsonAsync(result);
                }
                return response;
            });
        }

        [Function("WeatherForecastGet")]
        [OpenApiOperation(tags: new[] { "forecast" }, Summary = "Retrieve weather forecast")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "weatherForecastId", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: "populate", In = ParameterLocation.Query, Type = typeof(string[]))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WeatherForecast),
            Description = "Weather forecast")]
        public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Function, "get", Route = "weather/{weatherForecastId:required}")] HttpRequestData req,
                                                FunctionContext executionContext)
        {
            return await RequestWrapper(req, async response =>
            {
                string weatherForecastId = ExtractBindingData(executionContext)["weatherForecastId"].ToString();
                object populateObject;
                bool havePopulate = ExtractBindingData(executionContext).TryGetValue("populate", out populateObject);

                if (havePopulate)
                {
                    var result = this._weatherForecastRepository.Get(new ObjectId(weatherForecastId), populateObject.ToString().Split(","));
                    await response.WriteAsJsonAsync((object)result);
                }
                else
                {
                    var result = this._weatherForecastRepository.Get(new ObjectId(weatherForecastId));
                    await response.WriteAsJsonAsync(result);
                }
                return response;
            });
        }

        [Function("WeatherForecastPost")]
        [OpenApiOperation(tags: new[] { "forecast" }, Summary = "Create a new weather forecast")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(WeatherForecastRequest))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WeatherForecast),
            Description = "Created weather forecast")]
        public async Task<HttpResponseData> Post([HttpTrigger(AuthorizationLevel.Function, "post", Route = "weather")] HttpRequestData req,
                                                 FunctionContext executionContext)
        {
            return await RequestWrapper(req, async response =>
            {
                var weatherForecastRequest = DeserializeBody<WeatherForecastRequest, WeatherForecastRequestValidator>(req.Body);

                var result = this._weatherForecastRepository.Add(new WeatherForecast()
                {
                    Date = weatherForecastRequest.Date,
                    TemperatureC = weatherForecastRequest.TemperatureC,
                    Summary = weatherForecastRequest.Summary,
                    Reporter = weatherForecastRequest.Reporter
                });

                await response.WriteAsJsonAsync(result);
                return response;
            });
        }

        [Function("WeatherForecastPut")]
        [OpenApiOperation(tags: new[] { "forecast" }, Summary = "Update a weather forecast")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "weatherForecastId", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(WeatherForecastRequest))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WeatherForecast),
            Description = "Updated weather forecast")]
        public async Task<HttpResponseData> Put([HttpTrigger(AuthorizationLevel.Function, "put", Route = "weather/{weatherForecastId:required}")] HttpRequestData req,
                                                 FunctionContext executionContext)
        {
            return await RequestWrapper(req, async response =>
            {
                string weatherForecastId = ExtractBindingData(executionContext)["weatherForecastId"].ToString();

                var weatherForecastRequest = DeserializeBody<WeatherForecastRequest, WeatherForecastRequestValidator>(req.Body);
                var result = this._weatherForecastRepository.Update(new WeatherForecast()
                {
                    Id = weatherForecastId,
                    Date = weatherForecastRequest.Date,
                    TemperatureC = weatherForecastRequest.TemperatureC,
                    Summary = weatherForecastRequest.Summary,
                    Reporter = weatherForecastRequest.Reporter
                });

                await response.WriteAsJsonAsync(result);
                return response;
            });
        }

        [Function("WeatherForecastDelete")]
        [OpenApiOperation(tags: new[] { "forecast" }, Summary = "Delete a weather forecast")]
        [OpenApiParameter(name: "weatherForecastId", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WeatherForecast),
            Description = "Deleted weather forecast")]
        public async Task<HttpResponseData> Delete([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "weather/{weatherForecastId:required}")] HttpRequestData req,
                                                   FunctionContext executionContext)
        {
            return await RequestWrapper(req, async response =>
            {
                string weatherForecastId = ExtractBindingData(executionContext)["weatherForecastId"].ToString();
                var result = this._weatherForecastRepository.Delete(new ObjectId(weatherForecastId));
                await response.WriteAsJsonAsync(result);
                return response;
            });
        }
    }
}
