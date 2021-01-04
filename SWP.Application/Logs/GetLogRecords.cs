using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWP.Domain.Models.Portal;

namespace SWP.Application.Logs
{
    [TransientService]
    public class GetLogRecords
    {
        private readonly ILogManager _logManager;
        public GetLogRecords(ILogManager logManager) => _logManager = logManager;

        public List<Log> GetLogsByTypes(List<string> logTypes, DateTime startDate, DateTime endDate) =>
            _logManager.GetLogRecords(x => x, x => logTypes.Contains(x.Level) && x.TimeStamp >= startDate && x.TimeStamp <= endDate);

        public List<Log> GetLogsByTypesAndTags(List<string> logTypes, List<string> logTags, DateTime startDate, DateTime endDate) =>
            _logManager.GetLogRecords(x => x, x => logTypes.Contains(x.Level) && logTags.Any(y => x.Message.Contains(y)) && x.TimeStamp >= startDate && x.TimeStamp <= endDate);

        public List<Log> GetLogsByTags(List<string> logTags, DateTime startDate, DateTime endDate) =>
            _logManager.GetLogRecords(x => x, x => logTags.Any(y => x.Message.Contains(y)) && x.TimeStamp >= startDate && x.TimeStamp <= endDate);
    }
}
