using SWP.Domain.Models.Log;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Domain.Infrastructure
{
    public interface ILogManager
    {
        Task<LogRecord> CreateLogRecord(LogRecord record);
        Task<int> DeleteLogRecord(int id);
        LogRecord GetLogRecord(int id);
        List<TResult> GetLogRecordsWithSpecificData<TResult>(Func<LogRecord, TResult> selector);
        List<LogRecord> GetGetLogRecords();
        List<LogRecord> GetGetLogRecords(Func<LogRecord, bool> predicate);

        Task<Activity> CreateActivityRecord(Activity record);
        Task<int> DeleteActivityRecord(int id);
        Activity GetActivityRecord(int id);
        List<TResult> GetActivityRecordsWithSpecificData<TResult>(Func<Activity, TResult> selector);
        List<Activity> GetGetActivityRecords();
        List<Activity> GetGetActivityRecords(Func<Activity, bool> predicate);
    }
}
