using Microsoft.AspNetCore.Http;
using SWP.Application;
using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Models.News;
using System;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.News
{
    [TransientService]
    public class CreateNews
    {
        private readonly IFileManager _fileManager;
        private readonly INewsManager _newsManager;

        public CreateNews(INewsManager newsManager, IFileManager fileManager)
        {
            _fileManager = fileManager;
            _newsManager = newsManager;
        }

        public async Task<NewsRecord> CreateAsync(Request request)
        {
            var singleNews = new NewsRecord
            {
                Title = request.Title,
                Body = request.Body,
                Description = request.Description,
                Tags = request.Tags,
                Category = request.Category,
                Created = request.Created,
                CreatedBy = request.CreatedBy,
                Updated = request.Created,
                UpdatedBy = request.CreatedBy,

                Image = await _fileManager.SaveImageAsync(request.Image)
            };

            var result = await _newsManager.CreateOneNews(singleNews);
            return result;
        }

        public class Request
        {
            public string Title { get; set; }
            public string Body { get; set; }

            public string Description { get; set; }
            public string Tags { get; set; }
            public string Category { get; set; }

            public IFormFile Image { get; set; } = null;
            public DateTime Created { get; set; }
            public string CreatedBy { get; set; }
        }
    }
}
