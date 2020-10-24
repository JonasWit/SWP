using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SWP.UI.Automation
{
    class JobWakeUpCall : IJob
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public JobWakeUpCall(IHttpClientFactory httpFactory, IConfiguration configuration, IWebHostEnvironment env)
        {
            _httpFactory = httpFactory;
            _configuration = configuration;
            _env = env;
        }

        public Task Execute(IJobExecutionContext context)
        {
            using var client = _httpFactory.CreateClient();
            var uri = "";

            if (_env.IsProduction())
            {
                uri = _configuration["AutomationSettings:BaseUrlProd"];
            }
            else 
            {
                uri = _configuration["AutomationSettings:BaseUrlDev"];
            }

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Method = new HttpMethod("GET"),
                RequestUri = new Uri($"{uri}/automation-api/Automation/WakeUpCall")
            };

            httpRequestMessage.Headers.Add("ApiKey", _configuration["ApiKey"]);
            var response = client.SendAsync(httpRequestMessage).Result;

            return Task.CompletedTask;
        }
    }
}
