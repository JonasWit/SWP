using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP.Application.LegalSwp.News;
using SWP.UI.Pages.News.ViewModels;

namespace SWP.UI.Pages.News.Manager
{
    [Authorize(Roles = "Administrators")]
    public class EditModel : PageModel
    {
        [BindProperty]
        public NewsViewModel DataModel { get; set; } = new NewsViewModel();
        public string ActiveUserId { get; set; }

        public EditModel([FromServices] IHttpContextAccessor httpContextAccessor) =>
            ActiveUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public async Task<IActionResult> OnGet(int? id, [FromServices] GetOneNews getOneNews)
        {
            if (id == null)
            {
                return Page();
            }
            else
            {
                var singleNews = await Task.Run(() => getOneNews.Do((int)id));
                DataModel =  new NewsViewModel
                {
                    Id = singleNews.Id,
                    Title = singleNews.Title,
                    Body = singleNews.Body,
                    ImagePath = singleNews.Image,
                    Description = singleNews.Description,
                    Tags = singleNews.Tags,
                    Category = singleNews.Category
                };

                return Page();
            }
        }

        public IActionResult OnGetRemove(int id, [FromServices] DeleteNews deleteNews)
        {
            deleteNews.Delete(id);
            return RedirectToPage("/News/Manager/Index");
        }

        public async Task<IActionResult> OnPost([FromServices] CreateNews createNews, [FromServices] UpdateNews updateNews, [FromServices] UserManager<IdentityUser> userManager)
        {
            var user = await userManager.FindByIdAsync(ActiveUserId);
            var name = user.UserName;

            if (DataModel.Id > 0)
            {
                //Update post if it exists
                await updateNews.UpdateAsync(new UpdateNews.Request
                {
                    Id = DataModel.Id,
                    Title = DataModel.Title,
                    Body = DataModel.Body,
                    Image = DataModel.Image,
                    Updated = DateTime.Now,
                    UpdatedBy = name,
                    Description = DataModel.Description,
                    Tags = DataModel.Tags,
                    Category = DataModel.Category
                });;
            }
            else
            {
                //Create new post if not exists
                await createNews.CreateAsync(new CreateNews.Request
                {
                    Title = DataModel.Title,
                    Body = DataModel.Body,
                    Image = DataModel.Image,
                    Created = DateTime.Now,
                    CreatedBy = name,
                    Description = DataModel.Description,
                    Tags = DataModel.Tags,
                    Category = DataModel.Category
                });
            }

            return RedirectToPage("/News/Manager/Index");
        }




    }
}
