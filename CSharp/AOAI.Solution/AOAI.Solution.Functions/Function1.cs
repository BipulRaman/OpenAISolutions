using System.Net;
using System.Text.Json;
using AOAI.Solution.Functions.Helpers;
using AOAI.Solution.Functions.Models;
using AOAI.Solution.Helper.Abstractions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AOAI.Solution.Functions
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly ITextHelper textHelper;

        public Function1(ILoggerFactory loggerFactory, ITextHelper textHelper)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            this.textHelper = textHelper;
        }

        [Function("GetAnswer")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            string inputString = await req.ReadAsStringAsync().ConfigureAwait(false);

            string responseString = await textHelper.GetTextCompletionAsync(inputString).ConfigureAwait(false);

            response.WriteString(JsonSerializer.Serialize(responseString));

            return response;
        }

        [Function("GetEvaluation")]
        public async Task<HttpResponseData> GetEvaluation([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            Input input = await req.ReadFromJsonAsync<Input>();

            if(input is not null && input.IsValid())
            {
                string prompt = PromptHelper.GetPromptForEvaluation(input.Question, input.Answer, input.Fullmarks);
                string responseString = await textHelper.GetTextCompletionAsync(prompt).ConfigureAwait(false);
                response.WriteString(JsonSerializer.Serialize(responseString));
            }
            else
            {
                // Return bad request
                response = req.CreateResponse(HttpStatusCode.BadRequest);
            }

            return response;
        }
    }
}
