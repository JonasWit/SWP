using Microsoft.AspNetCore.Http;
using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Models.News;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SWP.Application.News
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

        public async Task<NewsRecord> UpdateAsync(Request request)
        {
            var oneNews = _newsManager.GetOneNews(request.Id, x => x);

            oneNews.Title = request.Title ?? oneNews.Title;
            oneNews.Body = request.Body ?? oneNews.Body;

            oneNews.Description = request.Description ?? oneNews.Description;
            oneNews.Tags = request.Tags ?? oneNews.Tags;
            oneNews.Category = request.Category ?? oneNews.Category;

            oneNews.UpdatedBy = oneNews.UpdatedBy;
            oneNews.Updated = oneNews.Updated;

            if (request.ImageStream is not null)
            {
                _fileManager.DeleteImage(oneNews.Image);
                oneNews.Image = await _fileManager.SaveImageAsync(request.ImageStream, request.ImageName);
            }

            var result = await _newsManager.UpdateOneNews(oneNews);
            return result;
        }

        public class Request
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }

            public string Description { get; set; }
            public string Tags { get; set; }
            public string Category { get; set; }

            public byte[] ImageStream { get; set; }
            public string ImageName { get; set; } = null;

            public DateTime Updated { get; set; }
            public string UpdatedBy { get; set; }
        }
    }
}
