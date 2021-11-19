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
    public class ReporterFunction : BaseFunction
    {
        private readonly IReporterRepository _reporterRepository;

        public ReporterFunction(IReporterRepository reporterRepository)
        {
            this._reporterRepository = reporterRepository;
        }

        [Function("ReporterList")]
        [OpenApiOperation(tags: new[] { "reporter" }, Summary = "Retrieve all reporters")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Reporter[]),
            Description = "All reporters")]
        public async Task<HttpResponseData> List([HttpTrigger(AuthorizationLevel.Function, "get", Route = "reporter")] HttpRequestData req,
                                                 FunctionContext executionContext)
        {
            return await RequestWrapper(req, async response =>
            {
                var result = this._reporterRepository.All();
                await response.WriteAsJsonAsync(result);
                return response;
            });
        }

        [Function("ReporterGet")]
        [OpenApiOperation(tags: new[] { "reporter" }, Summary = "Retrieve reporter")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "reporterId", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Reporter),
            Description = "Reporter")]
        public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Function, "get", Route = "reporter/{reporterId:required}")] HttpRequestData req,
                                                FunctionContext executionContext)
        {
            return await RequestWrapper(req, async response =>
            {
                string reporterId = ExtractBindingData(executionContext)["reporterId"].ToString();

                var result = this._reporterRepository.Get(new ObjectId(reporterId));
                await response.WriteAsJsonAsync(result);
                return response;
            });
        }

        [Function("ReporterPost")]
        [OpenApiOperation(tags: new[] { "reporter" }, Summary = "Create a new reporter")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ReporterRequest))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Reporter),
            Description = "Created reporter")]
        public async Task<HttpResponseData> Post([HttpTrigger(AuthorizationLevel.Function, "post", Route = "reporter")] HttpRequestData req,
                                                 FunctionContext executionContext)
        {
            return await RequestWrapper(req, async response =>
            {
                var reporterRequest = DeserializeBody<ReporterRequest, ReporterRequestValidator>(req.Body);

                var result = this._reporterRepository.Add(new Reporter()
                {
                    Firstname = reporterRequest.Firstname,
                    Middlename = reporterRequest.Middlename,
                    Lastname = reporterRequest.Lastname,
                    Age = reporterRequest.Age
                });

                await response.WriteAsJsonAsync(result);
                return response;
            });
        }

        [Function("ReporterPut")]
        [OpenApiOperation(tags: new[] { "reporter" }, Summary = "Update a reporter")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "reporterId", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ReporterRequest))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Reporter),
            Description = "Updated reporter")]
        public async Task<HttpResponseData> Put([HttpTrigger(AuthorizationLevel.Function, "put", Route = "reporter/{reporterId:required}")] HttpRequestData req,
                                                 FunctionContext executionContext)
        {
            return await RequestWrapper(req, async response =>
            {
                string reporterId = ExtractBindingData(executionContext)["reporterId"].ToString();

                var reporterRequest = DeserializeBody<ReporterRequest, ReporterRequestValidator>(req.Body);
                var result = this._reporterRepository.Update(new Reporter()
                {
                    Id = reporterId,
                    Firstname = reporterRequest.Firstname,
                    Middlename = reporterRequest.Middlename,
                    Lastname = reporterRequest.Lastname,
                    Age = reporterRequest.Age
                });

                await response.WriteAsJsonAsync(result);
                return response;
            });
        }

        [Function("ReporterDelete")]
        [OpenApiOperation(tags: new[] { "reporter" }, Summary = "Delete a reporter")]
        [OpenApiParameter(name: "reporterId", In = ParameterLocation.Path, Required = true, Type = typeof(string))]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Reporter),
            Description = "Deleted reporter")]
        public async Task<HttpResponseData> Delete([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "reporter/{reporterId:required}")] HttpRequestData req,
                                                   FunctionContext executionContext)
        {
            return await RequestWrapper(req, async response =>
            {
                string reporterId = ExtractBindingData(executionContext)["reporterId"].ToString();
                var result = this._reporterRepository.Delete(new ObjectId(reporterId));
                await response.WriteAsJsonAsync(result);
                return response;
            });
        }
    }
}
