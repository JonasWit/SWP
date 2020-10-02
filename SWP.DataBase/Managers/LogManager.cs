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
    public class LogManager : ILogManager
    {
        private readonly ApplicationDbContext context;
        public LogManager(ApplicationDbContext context) => this.context = context;

        public async Task<LogRecord> CreateLogRecord(LogRecord record)
        {
            context.LogRecords.Add(record);
            await context.SaveChangesAsync();
            return record;
        }

        public Task<int> DeleteLogRecord(int id)
        {
            context.LogRecords.Remove(context.LogRecords.FirstOrDefault(x => x.Id == id));
            return context.SaveChangesAsync();
        }

        public List<LogRecord> GetGetLogRecords() => context.LogRecords.ToList();

        public LogRecord GetLogRecord(int id) => context.LogRecords.FirstOrDefault(x => x.Id == id);

        public List<TResult> GetLogRecordSpecificData<TResult>(int id, Func<LogRecord, TResult> selector) => context.LogRecords.Select(selector).ToList();
    }
}
