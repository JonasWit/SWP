using Microsoft.AspNetCore.Mvc;
using SWP.Application.LegalSwp.News;
using System;

namespace WEB.Shop.UI.Controllers
{
    public class NewsController : Controller
    {
        [HttpGet("/image/{image}")]
        public IActionResult Image(string image, [FromServices] GetFile getFile)
        {
            try
            {
                FileStreamResult result = new FileStreamResult(getFile.Do(image), $"image/{image.Substring(image.LastIndexOf('.') + 1)}");
                return result;
            }
            catch (Exception)
            {
                return null;
            }   
        } 
    }
}