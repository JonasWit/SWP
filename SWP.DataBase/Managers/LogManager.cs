using SWP.Domain.Infrastructure;
using SWP.Domain.Models.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SWP.DataBase.Managers
{
    public class LogManager : DataManagerBase, ILogManager
    {
        public LogManager(ApplicationDbContext context) : base(context)
        {
        }

        #region Log

        public async Task<LogRecord> CreateLogRecord(LogRecord record)
        {
            if (_context.LogRecords.Where(x => x.UserId == "Automation").Count() > 10)
            {
                _context.LogRecords.RemoveRange(_context.LogRecords.Where(x => x.UserId == "Automation"));
            }

            _context.LogRecords.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public Task<int> DeleteLogRecord(int id)
        {
            _context.LogRecords.Remove(_context.LogRecords.FirstOrDefault(x => x.Id == id));
            return _context.SaveChangesAsync();
        }

        public List<LogRecord> GetLogRecords() => _context.LogRecords.ToList();

        public List<LogRecord> GetGetLogRecords(Func<LogRecord, bool> predicate) => _context.LogRecords.Where(predicate).ToList();

        public LogRecord GetLogRecord(int id) => _context.LogRecords.FirstOrDefault(x => x.Id == id);

        public List<TResult> GetLogRecordsWithSpecificData<TResult>(Func<LogRecord, TResult> selector) => _context.LogRecords.Select(selector).ToList();

        #endregion

        #region Activity

        public Activity GetActivityRecord(int id)
        {
            throw new NotImplementedException();
        }

        public List<TResult> GetActivityRecordsWithSpecificData<TResult>(Func<Activity, TResult> selector)
        {
            throw new NotImplementedException();
        }

        public List<Activity> GetGetActivityRecords()
        {
            throw new NotImplementedException();
        }

        public List<Activity> GetGetActivityRecords(Func<Activity, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateActivityRecord(Activity record)
        {
            _context.Activities.Add(record);
            return _context.SaveChangesAsync();
        }

        public Task<int> DeleteActivityRecord(int id)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
