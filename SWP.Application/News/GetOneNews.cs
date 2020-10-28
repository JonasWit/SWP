using SWP.Domain.Infrastructure.Portal;
using System;

namespace SWP.Application.News
{
    [TransientService]
    public class GetOneNews
    {
        private readonly INewsManager _newsManager;
        public GetOneNews(INewsManager newsManager) => _newsManager = newsManager;

        public Response Do(int id) =>
            _newsManager.GetOneNews(id, singleNews => new Response
            {
                Id = singleNews.Id,
                Title = singleNews.Title,
                Body = singleNews.Body,
                Image = singleNews.Image,
                Created = singleNews.Created,
                Description = singleNews.Description,
                Tags = singleNews.Tags,
                Category = singleNews.Category,
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

        public class Comment
        {
            public int Id { get; set; }
            public int NewsMainCommentId { get; set; }
            public string Message { get; set; }
            public DateTime Created { get; set; }
            public string Creator { get; set; }
        }
    }
}
