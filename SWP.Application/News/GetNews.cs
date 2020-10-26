using SWP.Domain.Infrastructure.Portal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SWP.Application.LegalSwp.News
{
    [TransientService]
    public class GetNews
    {
        private readonly INewsManager _newsManager;
        public GetNews(INewsManager newsManager) => _newsManager = newsManager;

        public IEnumerable<Response> Do() =>
            _newsManager.GetNews(singleNews => new Response
            {
                Id = singleNews.Id,
                Title = singleNews.Title,
                Body = singleNews.Body,
                Image = singleNews.Image,
                Created = singleNews.Created,
                Description = singleNews.Description,
                Tags = singleNews.Tags,
                Category = singleNews.Category
            });

        public int Count() => _newsManager.CountNews();

        public IEnumerable<Response> Do(int pageSize, int pageNumber) =>
            _newsManager.GetNews(pageSize, pageNumber, singleNews => new Response
            {
                Id = singleNews.Id,
                Title = singleNews.Title,
                Body = singleNews.Body,
                Image = singleNews.Image,
                Created = singleNews.Created,
                Description = singleNews.Description,
                Tags = singleNews.Tags,
                Category = singleNews.Category
            });

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
