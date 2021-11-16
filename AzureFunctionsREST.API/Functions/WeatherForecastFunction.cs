using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AzureFunctionsREST.Domain.Models;
using AzureFunctionsREST.Infrastructure.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace AzureFunctionsREST.API.Functions
{
    public class WeatherForecastFunction
    {
        private readonly WeatherForecastRepository _weatherForecastRepository;

        public WeatherForecastFunction(WeatherForecastRepository weatherForecastRepository)
        {
            this._weatherForecastRepository = weatherForecastRepository;
        }

        [Function("WeatherForecastGet")]
        [OpenApiOperation(tags: new[] { "forecast" }, Summary = "Retrieve all weather forecasts")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WeatherForecast[]),
            Description = "All weather forecasts")]
        public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Function, "get", Route = "weather")] HttpRequestData req,
                                                FunctionContext executionContext)
        {
            var result = this._weatherForecastRepository.All();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("WeatherForecastPost")]
        [OpenApiOperation(tags: new[] { "forecast" }, Summary = "Create a new weather forecast")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(WeatherForecast))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WeatherForecast),
            Description = "Created weather forecast")]
        public async Task<HttpResponseData> Post([HttpTrigger(AuthorizationLevel.Function, "post", Route = "weather")] HttpRequestData req,
                                                 FunctionContext executionContext)
        {
            WeatherForecast weatherForecast;
            using (StreamReader reader = new StreamReader(req.Body))
            {
                var bodyString = reader.ReadToEnd();
                weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(bodyString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            this._weatherForecastRepository.Add(weatherForecast);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(weatherForecast);

            return response;
        }
    }
}
