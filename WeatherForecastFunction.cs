using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using System.Net;
using azure_functions_dotnet_rest_api.Models;
using azure_functions_dotnet_rest_api.Services;

namespace azure_functions_dotnet_rest_api
{
    public class WeatherForecastFunction
    {
        private readonly WeatherForecastService _weatherForecastService;

        public WeatherForecastFunction(WeatherForecastService weatherForecastService)
        {
            this._weatherForecastService = weatherForecastService;
        }

        [FunctionName("WeatherForecast")]
        [OpenApiOperation(operationId: "Run")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(WeatherForecast),
            Description = "The OK response message containing a JSON result.")]
        public async Task<IActionResult> WeatherForecastFn(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "weather")] HttpRequest req,
            ILogger log)
        {
            var result = await this._weatherForecastService.Predict();
            return new OkObjectResult(result);
        }
    }
}
