using SWP.Domain.Models.News;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP.Domain.Infrastructure.Portal
{
    public interface INewsManager //todo:add implementation
    {
        Task<int> CreateOneNews(NewsRecord news);
        Task<int> UpdateOneNews(NewsRecord news);
        int DeleteOneNews(int id);
        int CountNews();
        TResult GetOneNews<TResult>(int id, Func<NewsRecord, TResult> selector);
        IEnumerable<TResult> GetNews<TResult>(Func<NewsRecord, TResult> selector);
        IEnumerable<TResult> GetNews<TResult>(int pageSize, int pageNumber, Func<NewsRecord, TResult> selector);
    }
}
