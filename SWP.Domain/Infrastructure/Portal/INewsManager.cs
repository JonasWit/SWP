using SWP.Domain.Models.News;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP.Domain.Infrastructure.Portal
{
    public interface INewsManager //todo:add implementation
    {
        Task<NewsRecord> CreateOneNews(NewsRecord news);
        Task<NewsRecord> UpdateOneNews(NewsRecord news);
        int DeleteOneNews(int id);

        int CountNews(string category);
        int CountNews();

        TResult GetOneNews<TResult>(int id, Func<NewsRecord, TResult> selector);

        IEnumerable<TResult> GetNews<TResult>(string category, Func<NewsRecord, TResult> selector, Func<NewsRecord, bool> predicate);
        IEnumerable<TResult> GetNews<TResult>(Func<NewsRecord, TResult> selector);

        IEnumerable<TResult> GetNews<TResult>(int pageSize, int pageNumber, string category, Func<NewsRecord, TResult> selector, Func<NewsRecord, bool> predicate);
        IEnumerable<TResult> GetNews<TResult>(int pageSize, int pageNumber, Func<NewsRecord, TResult> selector);
    }
}
