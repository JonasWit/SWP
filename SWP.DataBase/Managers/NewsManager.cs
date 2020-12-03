using Microsoft.EntityFrameworkCore;
using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Models.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.DataBase.Managers
{
    public class NewsManager : DataManagerBase, INewsManager
    {
        public NewsManager(AppContext context) : base(context)
        {
        }

        public int CountNews(string category) => _context.News.Count(news => news.Category == category);
        
        public int CountNews() => _context.News.Count();

        public async Task<NewsRecord> CreateOneNews(NewsRecord news)
        {
            _context.News.Add(news);
            await _context.SaveChangesAsync();
            return news;
        }

        public int DeleteOneNews(int id)
        {
            var news = _context.News.FirstOrDefault(x => x.Id == id);
            _context.News.Remove(news);
            return _context.SaveChanges();
        }

        public async Task<NewsRecord> UpdateOneNews(NewsRecord news)
        {
            _context.News.Update(news);
            await _context.SaveChangesAsync();
            return news;
        }

        public TResult GetOneNews<TResult>(int id, Func<NewsRecord, TResult> selector) =>
            _context.News
                .Where(x => x.Id == id)
                .Select(selector)
                .FirstOrDefault();

        public IEnumerable<TResult> GetNews<TResult>(Func<NewsRecord, TResult> selector) =>
            _context.News
                .Select(selector)
                .ToList();

        public IEnumerable<TResult> GetNews<TResult>(string category, Func<NewsRecord, TResult> selector, Func<NewsRecord, bool> predicate) =>
            _context.News
                .Where(predicate)
                .Select(selector)
                .ToList();

        public IEnumerable<TResult> GetNews<TResult>(int pageSize, int pageNumber, string category, Func<NewsRecord, TResult> selector, Func<NewsRecord, bool> predicate) => _context.News
                .Where(predicate)
                .Select(selector)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();
  
        public IEnumerable<TResult> GetNews<TResult>(int pageSize, int pageNumber, Func<NewsRecord, TResult> selector) => 
            _context.News
               .Select(selector)
               .Skip(pageSize * (pageNumber - 1))
               .Take(pageSize)
               .ToList();

    }
}
