using SWP.Domain.Models.Portal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP.Domain.Infrastructure
{
    public interface ILogManager
    {
        List<TResult> GetLogRecords<TResult>(Func<Log, TResult> selector, Func<Log, bool> predicate);

        Task<int> DeleteLogRecord(int id);
    }
}
