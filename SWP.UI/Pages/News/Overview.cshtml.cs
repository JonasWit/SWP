using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP.Application.News;
using SWP.UI.Pages.News.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SWP.UI.Pages.News
{
    public class IndexModel : PageModel
    {
        public NewsPageViewModel DataModel { get; set; }
        public int PageSize { get; set; } = 5;

        public IActionResult OnGet(int pageNumber, string category, [FromServices] GetNews getNews)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var news = string.IsNullOrEmpty(category) ? getNews.Do(PageSize, pageNumber) : getNews.Do(PageSize, pageNumber, category);
            int newsCount = string.IsNullOrEmpty(category) ? getNews.Count() : getNews.Count(category);

            int skipAmount = PageSize * (pageNumber - 1);
            int capacity = skipAmount + PageSize;

            var newsVm = new List<NewsViewModel>();

            foreach (var singleNews in news)
            {
                newsVm.Add(new NewsViewModel
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

            newsVm = newsVm.OrderByDescending(x => x.Created).ToList();

            DataModel = new NewsPageViewModel
            {
                News = newsVm,
                PageNumber = pageNumber,
                Category = category,
                PageCount = (int)Math.Ceiling((double)newsCount / PageSize)
            };

            if (newsCount > capacity)
            {
                DataModel.NextPage = true;
            }

            return Page();
        }

       
    }
}
