using System.Net;
using System.Threading.Tasks;
using AzureFunctionsREST.API.Models;
using AzureFunctionsREST.API.Validators;
using AzureFunctionsREST.Domain.Interfaces;
using AzureFunctionsREST.Domain.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;

namespace AzureFunctionsREST.API.Functions
{
    public class StationFunction : BaseFunction
    {
        private readonly IStationRepository _stationRepository;

        public StationFunction(IStationRepository stationRepository)
        {
            this._stationRepository = stationRepository;
        }

        [Function("StationList")]
        [OpenApiOperation(tags: new[] { "station" }, Summary = "Retrieve all stations")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Station[]),
            Description = "All stations")]
        public async Task<HttpResponseData> List([HttpTrigger(AuthorizationLevel.Function, "get", Route = "station")] HttpRequestData req,
                                                 FunctionContext executionContext)
        {
            return await RequestWrapper(req, async response =>
            {
                var result = this._stationRepository.All();
                await response.WriteAsJsonAsync(result);
                return response;
            });
        }

        [Function("StationGet")]
        [OpenApiOperation(tags: new[] { "station" }, Summary = "Retrieve station")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "stationId", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Station),
            Description = "station")]
        public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Function, "get", Route = "station/{stationId:required}")] HttpRequestData req,
                                                FunctionContext executionContext)
        {
            return await RequestWrapper(req, async response =>
            {
                string stationId = ExtractBindingData(executionContext)["stationId"].ToString();

                var result = this._stationRepository.Get(new ObjectId(stationId));
                await response.WriteAsJsonAsync(result);
                return response;
            });
        }

        [Function("StationPost")]
        [OpenApiOperation(tags: new[] { "station" }, Summary = "Create a new station")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(StationRequest))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Station),
            Description = "Created station")]
        public async Task<HttpResponseData> Post([HttpTrigger(AuthorizationLevel.Function, "post", Route = "station")] HttpRequestData req,
                                                 FunctionContext executionContext)
        {
            return await RequestWrapper(req, async response =>
            {
                var stationRequest = DeserializeBody<StationRequest, StationRequestValidator>(req.Body);

                var result = this._stationRepository.Add(new Station()
                {
                    Name = stationRequest.Name,
                    Location = stationRequest.Location
                });

                await response.WriteAsJsonAsync(result);
                return response;
            });
        }

        [Function("StationPut")]
        [OpenApiOperation(tags: new[] { "station" }, Summary = "Update a station")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "stationId", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(StationRequest))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Station),
            Description = "Updated station")]
        public async Task<HttpResponseData> Put([HttpTrigger(AuthorizationLevel.Function, "put", Route = "station/{stationId:required}")] HttpRequestData req,
                                                 FunctionContext executionContext)
        {
            return await RequestWrapper(req, async response =>
            {
                string stationId = ExtractBindingData(executionContext)["stationId"].ToString();

                var stationRequest = DeserializeBody<StationRequest, StationRequestValidator>(req.Body);
                var result = this._stationRepository.Update(new Station()
                {
                    Name = stationRequest.Name,
                    Location = stationRequest.Location
                });

                await response.WriteAsJsonAsync(result);
                return response;
            });
        }

        [Function("StationDelete")]
        [OpenApiOperation(tags: new[] { "station" }, Summary = "Delete a station")]
        [OpenApiParameter(name: "stationId", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Station),
            Description = "Deleted station")]
        public async Task<HttpResponseData> Delete([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "station/{stationId:required}")] HttpRequestData req,
                                                   FunctionContext executionContext)
        {
            return await RequestWrapper(req, async response =>
            {
                string stationId = ExtractBindingData(executionContext)["stationId"].ToString();
                var result = this._stationRepository.Delete(new ObjectId(stationId));
                await response.WriteAsJsonAsync(result);
                return response;
            });
        }
    }
}
