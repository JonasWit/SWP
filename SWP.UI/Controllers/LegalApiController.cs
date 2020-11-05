using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SWP.UI.Controllers
{
    [Route("LegalAppSpecificAPI/[controller]")]
    [Authorize(Roles = "Users, Administrators")]
    public class LegalApiController
    {
        private readonly IHttpClientFactory _httpFactory;

        public LegalApiController(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        [HttpGet("CourtsList")]
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
