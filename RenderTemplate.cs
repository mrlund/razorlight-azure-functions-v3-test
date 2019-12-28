using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RazorLight;
using System.Reflection;

namespace RazorFuncTest
{
    public static class RenderTemplate
    {
        [FunctionName("RenderTemplate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            var engine = new RazorLightEngineBuilder()
                .SetOperatingAssembly(Assembly.GetExecutingAssembly())
                .UseEmbeddedResourcesProject(typeof(RenderTemplate))
              .UseMemoryCachingProvider()
              .Build();

            string template = "Hello, @Model.Name. Welcome to RazorLight repository at @Model.Time";
            var model = new { Name = "John Doe", Time = DateTime.UtcNow };

            string result = await engine.CompileRenderStringAsync("templateKey", template, model);


            return new OkObjectResult(result);
        }
    }
}
