using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SWP.Application.Maintenance;
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

        [HttpGet("CleanupCall")]
        public async Task<IActionResult> CleanupCall(
            [FromServices] ILogger<AutomationController> logger, 
            [FromServices] RunPortalMaintenance runPortalMaintenance)
        {  
            var result = await runPortalMaintenance.RunFullCleanup();

            //var resuest = new CreateLogRecord.Request { Message = "Crawlers Started", TimeStamp = DateTime.Now };
            //await createLogRecord.DoAsync(resuest);

            //var productsFound = await crawlersCommander.RunEngineAsync();

            //resuest = new CreateLogRecord.Request { Message = $"Scraping Finished, products found: {productsFound}", TimeStamp = DateTime.Now };
            //await createLogRecord.DoAsync(resuest);

            logger.LogInformation($"{LogTags.AutomationLogPrefix} Clean Up Finished.");
            return Ok(true);
        }
    }
}
