using Microsoft.AspNetCore.Mvc;
using SWP.UI.Utilities;

namespace SWP.UI.Controllers
{
    //API Key auth Example
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class LicensesController : ControllerBase
    {

    }
}
