using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace api
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> log)
        {
            _logger = log;
        }

        [FunctionName("Function1")]
        [OpenApiOperation(operationId: "RunFunction1", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> RunFunction1(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // Response
            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("Function2")]
        [OpenApiOperation(operationId: "RunFunction2")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> RunFunction2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // Create blob client
            var connectionStr = Environment.GetEnvironmentVariable("StorageAccountConnectionString");
            const string containerName = "testcontainer";
            var blobContainerClient = new BlobContainerClient(connectionStr, containerName);
            await blobContainerClient.CreateIfNotExistsAsync();

            // Get a reference to a blob in a container 
            const string blobName = "1.json";
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            // Upload the content
            var testObj = new TestObject() { TestProperty = DateAndTime.Now.ToLongDateString() };
            var byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(testObj));
            await using var memoryStream = new MemoryStream(byteArray);
            await blobClient.UploadAsync(memoryStream, overwrite: true);

            return new OkObjectResult("Completed");
        }

        [FunctionName("Function3")]
        [OpenApiOperation(operationId: "RunFunction3")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IEnumerable<TestObject>> RunFunction3(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var list = new List<TestObject>
            {
                new TestObject()
                {
                    TestProperty = DateAndTime.Now.ToLongDateString()
                }
            };
            return list;
        }

        public class TestObject
        {
            public string TestProperty { get; set; }
        }
    }
}

