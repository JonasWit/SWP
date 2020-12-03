using Microsoft.AspNetCore.Mvc;
using SWP.UI.Utilities;
using System.Threading.Tasks;

namespace SWP.UI.Controllers
{
    [Route("AutomationAPI/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class AutomationController : ControllerBase
    {
        [HttpGet("WakeUpCall")]
        public async Task<int> WakeUpCheck()
        {
            //todo:add logging!

            //var request = new CreateLogRecord.AutomationRequest { Action = "Wake Up Call", TimeStamp = DateTime.Now };
            //await portalLogger.CreateLogRecord(request);
            return 1;
        }
    }
}
