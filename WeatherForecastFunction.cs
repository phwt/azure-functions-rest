using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using azure_functions_dotnet_rest_api.Models;
using azure_functions_dotnet_rest_api.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace azure_functions_dotnet_rest_api
{
    public class WeatherForecastFunction
    {
        private readonly WeatherForecastService _weatherForecastService;

        public WeatherForecastFunction(WeatherForecastService weatherForecastService)
        {
            this._weatherForecastService = weatherForecastService;
        }

        [Function("WeatherForecastGet")]
        [OpenApiOperation(tags: new[] { "forecast" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WeatherForecast[]),
            Description = "The OK response message containing a JSON result.")]
        public async Task<HttpResponseData> WeatherForecastGet([HttpTrigger(AuthorizationLevel.Function, "get", Route = "weather")] HttpRequestData req,
                                                        FunctionContext executionContext)
        {
            var result = await this._weatherForecastService.Predict();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("WeatherForecastPost")]
        [OpenApiOperation(tags: new[] { "forecast" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBodyAttribute(contentType: "application/json", bodyType: typeof(WeatherForecast))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WeatherForecast),
            Description = "The OK response message containing a JSON result.")]
        public async Task<HttpResponseData> WeatherForecastPost([HttpTrigger(AuthorizationLevel.Function, "post", Route = "weather")] HttpRequestData req,
                                                        FunctionContext executionContext)
        {
            var result = await this._weatherForecastService.Predict();

            WeatherForecast body;
            using (StreamReader reader = new StreamReader(req.Body))
            {
                var bodyString = reader.ReadToEnd();
                body = JsonSerializer.Deserialize<WeatherForecast>(bodyString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(body);

            return response;
        }
    }
}
