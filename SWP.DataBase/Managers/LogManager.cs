using SWP.Domain.Infrastructure;
using SWP.Domain.Models.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.DataBase.Managers
{
    public class LogManager : DataManagerBase, ILogManager
    {
        public LogManager(AppContext context) : base(context) { }

        public Task<int> DeleteLogRecord(int id)
        {
            var entity = _context.Logs.FirstOrDefault(x => x.Id == id);
            _context.Logs.Remove(entity);
            return _context.SaveChangesAsync();
        }

        public List<TResult> GetLogRecords<TResult>(Func<Log, TResult> selector, Func<Log, bool> predicate) =>
            _context.Logs.Where(predicate).Select(selector).ToList();
    }
}
