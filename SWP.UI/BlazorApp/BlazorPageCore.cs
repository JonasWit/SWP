using SWP.Application.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SWP.Domain.Models.Log;

namespace SWP.UI.BlazorApp
{
    public abstract class BlazorPageCore
    {
        protected readonly IServiceProvider _serviceProvider;

        private CreateLogRecord CreateLogEntry => _serviceProvider.GetService<CreateLogRecord>();
        private DeleteLogRecord DeleteLogRecord => _serviceProvider.GetService<DeleteLogRecord>();
        private GetLogRecord GetLogRecord => _serviceProvider.GetService<GetLogRecord>();
        private GetLogRecords GetLogRecords => _serviceProvider.GetService<GetLogRecords>();

        public BlazorPageCore(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        protected Task<LogRecord> CreateLog(string userId, string message, string stack) => 
            CreateLogEntry.Create(new CreateLogRecord.Request
            {
                Message = message,
                UserId = userId,
                StackTrace = stack
            });

        protected List<LogRecord> GetCleanLogs() => GetLogRecords.GetRecordsWitohutStacks();

        protected List<LogRecord> GetLogs() => GetLogRecords.GetRecords();

        protected LogRecord GetLog(int id) => GetLogRecord.Get(id);

        protected Task<int> DeleteLog(int id) => DeleteLogRecord.Delete(id);
    }
}
