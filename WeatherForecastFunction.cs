using System.Net;
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

        [Function("WeatherForecast")]
        [OpenApiOperation(operationId: "Run")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WeatherForecast),
            Description = "The OK response message containing a JSON result.")]
        public async Task<HttpResponseData> WeatherForecastFn([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "weather")] HttpRequestData req,
                                                        FunctionContext executionContext)
        {
            var result = await this._weatherForecastService.Predict();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
