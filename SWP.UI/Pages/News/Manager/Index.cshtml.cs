using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP.Application.LegalSwp.News;
using SWP.UI.Pages.News.ViewModels;

namespace SWP.UI.Pages.News.Manager
{
    [Authorize(Roles = "Administrators")]
    public class IndexModel : PageModel
    {
        public List<NewsViewModel> DataModels { get; set; } = new List<NewsViewModel>();

        public void OnGet([FromServices] GetNews getNews)
        {
            var news = getNews.Do();

            foreach (var singleNews in news)
            {
                DataModels.Add(new NewsViewModel
                {
                    Id = singleNews.Id,
                    Title = singleNews.Title,
                    Body = singleNews.Body,
                    ImagePath = singleNews.Image,
                    Created = singleNews.Created,
                    Description = singleNews.Description,
                    Tags = singleNews.Tags,
                    Category = singleNews.Category
                });
            }
        }
    }
}
