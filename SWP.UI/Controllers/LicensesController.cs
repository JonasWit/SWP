using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP.Application.Licenses;
using SWP.UI.Utilities;

namespace SWP.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class LicensesController : ControllerBase
    {
        [HttpPost("ValidateLicense")]
        public async Task<ValidateLicense.Response> RunCrawlers([FromBody] ValidateLicense.Request request)
        {
            var response = new ValidateLicense.Response();

            return response;
        }
    }
}
