using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace api
{
    public class OperationHttpTrigger
    {
        private readonly ILogger<OperationHttpTrigger> _logger;

        public OperationHttpTrigger(ILogger<OperationHttpTrigger> log)
        {
            _logger = log;
        }

        [FunctionName(nameof(OperationHttpTrigger.AddOperation))]
        [OpenApiOperation(operationId: "addOperation", tags: new[] { "operation" }, Summary = "Add a new operation", Description = "This add a new operation", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Operation), Required = true, Description = "Operation object that needs to be added")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Operation), Summary = "New operation details added", Description = "New operation details added")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
        public async Task<IActionResult> AddOperation(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "operation")] HttpRequest req)
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();

            Operation operation= JsonConvert.DeserializeObject<Operation>(content);

            return await Task.FromResult(new OkObjectResult(operation)).ConfigureAwait(false);
        }

        [FunctionName(nameof(OperationHttpTrigger.UpdateOperation))]
        [OpenApiOperation(operationId: "updateOperation", tags: new[] { "operation" }, Summary = "Update an existing operation", Description = "This updates an existing operation.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Operation), Required = true, Description = "Operation object that needs to be updated")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Operation), Summary = "Operation details updated", Description = "Operation details updated")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Operation not found", Description = "Operation not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        public async Task<IActionResult> UpdateOperation(
            [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "operation")] HttpRequest req)
        {
            var kk = new Operation();
            kk.Name = "Kk";


            return await Task.FromResult(new OkObjectResult(kk)).ConfigureAwait(false);
        }

        [FunctionName(nameof(OperationHttpTrigger.GetOperationById))]
        [OpenApiOperation(operationId: "getOperationById", tags: new[] { "operation" }, Summary = "Find operation by ID", Description = "Returns a single operation.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "operationId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "ID of operation to return", Description = "ID of operation to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Operation), Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Operation not found", Description = "Operation not found")]
        public async Task<IActionResult> GetOperationById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "operation/{operationId}")] HttpRequest req, long operationId)
        {
            var kk = new Operation();
            kk.Name = "Kk";


            return await Task.FromResult(new OkObjectResult(kk)).ConfigureAwait(false);
        }

        [FunctionName(nameof(OperationHttpTrigger.DeleteOperation))]
        [OpenApiOperation(operationId: "deleteOperation", tags: new[] { "operation" }, Summary = "Deletes a operation", Description = "This deletes a operation.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "operationId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "Operation id to delete", Description = "Operation id to delete", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Summary = "successful operation", Description = "successful operation")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Operation not found", Description = "Operation not found")]
        public async Task<IActionResult> DeleteOperation(
        [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "operation/{operationId}")] HttpRequest req, long operationId)
        {
            return await Task.FromResult(new OkResult()).ConfigureAwait(false);
        }

        ////[FunctionName(nameof(OperationHttpTrigger.FindByStatus))]
        ////[OpenApiOperation(operationId: "findOperationsByStatus", tags: new[] { "operation" }, Summary = "Finds operations by status", Description = "Multiple status values can be provided with comma separated strings.", Visibility = OpenApiVisibilityType.Important)]
        ////[OpenApiParameter(name: "status", In = ParameterLocation.Query, Required = true, Type = typeof(List<PetStatus>), Explode = true, Summary = "Operation status value", Description = "Status values that need to be considered for filter", Visibility = OpenApiVisibilityType.Important)]
        ////[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Operation>), Summary = "successful operation", Description = "successful operation")]
        ////[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid status value", Description = "Invalid status value")]
        ////public async Task<IActionResult> FindByStatus(
        ////    [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "operation/findByStatus")] HttpRequest req)
        ////{
        ////    //var status = req.Query["status"]
        ////    //    .Select(p =>
        ////    //    {
        ////    //        var converted = Enum.TryParse<PetStatus>(p, ignoreCase: true, out var result) ? result : PetStatus.Available;
        ////    //        return converted;
        ////    //    })
        ////    //    .ToList();
        ////    //var pets = this._fixture.Create<List<Pet>>().Where(p => status.Contains(p.Status));

        ////    return await Task.FromResult(new OkObjectResult(pets)).ConfigureAwait(false);
        ////}
    }
}

