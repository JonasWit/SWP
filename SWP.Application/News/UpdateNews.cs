using Microsoft.AspNetCore.Http;
using SWP.Domain.Infrastructure.Portal;
using System;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.News
{
    [TransientService]
    public class UpdateNews
    {
        private readonly INewsManager _newsManager;
        private readonly IFileManager _fileManager;

        public UpdateNews(INewsManager newsManager, IFileManager fileManager)
        {
            _fileManager = fileManager;
            _newsManager = newsManager;
        }

        public async Task<Response> UpdateAsync(Request request)
        {
            var oneNews = _newsManager.GetOneNews(request.Id, x => x);

            oneNews.Title = request.Title ?? oneNews.Title;
            oneNews.Body = request.Body ?? oneNews.Body;

            oneNews.Description = request.Description ?? oneNews.Description;
            oneNews.Tags = request.Tags ?? oneNews.Tags;
            oneNews.Category = request.Category ?? oneNews.Category;

            if (request.Image != null)
            {
                _fileManager.DeleteImage(oneNews.Image);
                oneNews.Image = await _fileManager.SaveImageAsync(request.Image);
            }

            await _newsManager.UpdateOneNews(oneNews);

            return new Response
            {
                Id = oneNews.Id,
                Title = oneNews.Title,
                Body = oneNews.Body,
                Image = oneNews.Image,
                Created = oneNews.Created,
                Description = oneNews.Description,
                Tags = oneNews.Tags,
                Category = oneNews.Category,
            };
        }

        public class Request
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }

            public string Description { get; set; }
            public string Tags { get; set; }
            public string Category { get; set; }

            public IFormFile Image { get; set; } = null;
            public DateTime Created { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }

            public string Description { get; set; }
            public string Tags { get; set; }
            public string Category { get; set; }

            public string Image { get; set; } = null;
            public DateTime Created { get; set; }
        }
    }
}
