using Microsoft.AspNetCore.Mvc;
using SWP.Application.Log;
using SWP.UI.Utilities;
using System;
using System.Threading.Tasks;

namespace SWP.UI.Controllers
{
    [Route("AutomationAPI/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class AutomationController : ControllerBase
    {
        [HttpGet("WakeUpCall")]
        public async Task<CreateLogRecord.AutomationRequest> WakeUpCheck([FromServices] CreateLogRecord createLogRecord)
        {
            var request = new CreateLogRecord.AutomationRequest { Action = "Wake Up Call", TimeStamp = DateTime.Now };
            await createLogRecord.CreateAutomationRecord(request);
            return request;
        }
    }
}
