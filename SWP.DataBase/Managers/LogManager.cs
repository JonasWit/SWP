using SWP.Domain.Infrastructure;
using SWP.Domain.Models.Log;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.DataBase.Managers
{
    public class LogManager : ILogManager
    {
        public Task<LogRecord> CreateLogRecord(LogRecord client)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteLogRecord(int id)
        {
            throw new NotImplementedException();
        }

        public List<LogRecord> GetGetLogRecords()
        {
            throw new NotImplementedException();
        }

        public List<LogRecord> GetGetLogRecordsWithoutTraces()
        {
            throw new NotImplementedException();
        }

        public LogRecord GetLogRecord(int id)
        {
            throw new NotImplementedException();
        }

        public TResult GetLogRecordSpecificData<TResult>(int id, Func<LogRecord, TResult> selector)
        {
            throw new NotImplementedException();
        }
    }
}
