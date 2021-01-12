using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SWP.UI.Automation
{
    public class JobCleanupCall : IJob
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly IConfiguration _configuration;

        public JobCleanupCall(IHttpClientFactory httpFactory, IConfiguration configuration)
        {
            _httpFactory = httpFactory;
            _configuration = configuration;
        }

        public Task Execute(IJobExecutionContext context)
        {
            using var client = _httpFactory.CreateClient();
            var uri = _configuration["AutomationSettings:BaseAutomationUrl"];

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Method = new HttpMethod("GET"),
                RequestUri = new Uri($"{uri}/AutomationAPI/Automation/CleanupCall")
            };

            httpRequestMessage.Headers.Add("ApiKey", _configuration["ApiKey"]);
            var response = client.SendAsync(httpRequestMessage).Result;

            return Task.CompletedTask;
        }
    }
}
