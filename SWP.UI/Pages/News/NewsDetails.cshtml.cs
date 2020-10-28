using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP.Application.News;
using SWP.UI.Pages.News.ViewModels;

namespace SWP.UI.Pages.News
{
    public class NewsDispalyModel : PageModel
    {
        public NewsViewModel DataModel { get; set; } = new NewsViewModel();

        public void OnGet(int id, [FromServices] GetOneNews getOneNews)
        {
            var singleNews = getOneNews.Do(id);
            DataModel = new NewsViewModel
            {
                Id = singleNews.Id,
                Title = singleNews.Title,
                Body = singleNews.Body,
                ImagePath = singleNews.Image,
                Created = singleNews.Created,
                Description = singleNews.Description,
                Tags = singleNews.Tags,
                Category = singleNews.Category
            };
        }
    }
}
