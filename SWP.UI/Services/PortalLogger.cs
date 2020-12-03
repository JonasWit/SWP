using SWP.Application.Log;
using SWP.Domain.Models.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Services
{
    [UITransientService]
    public class PortalLogger
    {
        private readonly CreateLogRecord _createLogRecord;
        private readonly DeleteLogRecord _deleteLogRecord;
        private readonly GetLogRecord _getLogRecord;
        private readonly GetLogRecords _getLogRecords;

        public PortalLogger(
            CreateLogRecord createLogRecord,
            DeleteLogRecord deleteLogRecord,
            GetLogRecord getLogRecord,
            GetLogRecords getLogRecords)
        {
            _createLogRecord = createLogRecord;
            _deleteLogRecord = deleteLogRecord;
            _getLogRecord = getLogRecord;
            _getLogRecords = getLogRecords;
        }

        public Task<LogRecord> LogException(Exception ex, string userId, string customMessage = "Log Entry") => 
            _createLogRecord.Create(userId, $"{customMessage}-{ex.Message}", ex.StackTrace);

        public Task<LogRecord> LogException(Exception ex, string userId) =>
             _createLogRecord.Create(new CreateLogRecord.Request
             {
                 Message = ex.Message,
                 UserId = userId,
                 StackTrace = ex.StackTrace
             });

        public List<LogRecord> GetLogRecords() => _getLogRecords.GetRecords();

        public List<LogRecord> GetLogRecordsWitohutStacks() => _getLogRecords.GetRecordsWitohutStacks();

        public LogRecord GetLogRecord(int id) => _getLogRecord.Get(id);

        public Task<int> DeleteLogRecord(int id) => _deleteLogRecord.Delete(id);

        public Task<LogRecord> CreateLogRecord(string userId, string message, string stack) => _createLogRecord.Create(userId, message, stack);

        public Task<LogRecord> CreateLogRecord(CreateLogRecord.Request request) => _createLogRecord.Create(request);

        public Task<LogRecord> CreateLogRecord(CreateLogRecord.AutomationRequest request) => _createLogRecord.Create(request);
    }
}
