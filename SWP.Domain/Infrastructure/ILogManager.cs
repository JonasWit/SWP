using SWP.Domain.Models.Log;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Domain.Infrastructure
{
    public interface ILogManager
    {
        Task<LogRecord> CreateLogRecord(LogRecord client);

        Task<int> DeleteLogRecord(int id);

        LogRecord GetLogRecord(int id);

        TResult GetLogRecordSpecificData<TResult>(int id, Func<LogRecord, TResult> selector);

        List<LogRecord> GetGetLogRecords();

        List<LogRecord> GetGetLogRecordsWithoutTraces();
    }
}
