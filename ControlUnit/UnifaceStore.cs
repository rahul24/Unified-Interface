using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Google.Protobuf;
using ControlUnit.Common.Model;
using ControlUnit.Intent.lib;
using ControlUnit.Intent;

namespace ControlUnit
{
    public static class UnifaceStore
    {
        private static readonly JsonParser jsonParser =
            new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));

        [FunctionName("UniFaceStore")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            CommonModel model = RequestHandler.Handle(requestBody);


            var intent = Commonfunc.GetInstance<IIntent>(model.Intent);
            intent.Process(model);

            string responseBody = ResponseHandler.Handle(model);
           

            return !string.IsNullOrEmpty(responseBody)
                ? (ActionResult)new OkObjectResult(responseBody)
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
