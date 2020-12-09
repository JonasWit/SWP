using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public IActionResult WakeUpCheck([FromServices] ILogger<AutomationController> logger)
        {
            logger.LogInformation($"{LogTags.AutomationLogPrefix} Wake Up Call.");
            return Ok(true);
        }
    }
}
