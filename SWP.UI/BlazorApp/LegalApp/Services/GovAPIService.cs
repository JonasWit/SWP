using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Services
{
    [UITransientService]
    public class GovAPIService
    {
        private readonly IHttpClientFactory _httpFactory;

        public GovAPIService(IHttpClientFactory httpFactory) => _httpFactory = httpFactory;

        public Task GetCoutrsList()
        {
            using var client = _httpFactory.CreateClient();
            var uri = "";

            //HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            //{
            //    Method = new HttpMethod("GET"),
            //    RequestUri = new Uri($"{uri}/AutomationAPI/Automation/WakeUpCall")
            //};

            //httpRequestMessage.Headers.Add("ApiKey", _configuration["ApiKey"]);
            //var response = client.SendAsync(httpRequestMessage).Result;

            return Task.CompletedTask;
        }







    }
}
