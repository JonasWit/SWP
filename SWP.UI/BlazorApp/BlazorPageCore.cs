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
        protected readonly IServiceProvider serviceProvider;

        private CreateLogRecord CreateLogEntry => serviceProvider.GetService<CreateLogRecord>();
        private DeleteLogRecord DeleteLogRecord => serviceProvider.GetService<DeleteLogRecord>();
        private GetLogRecord GetLogRecord => serviceProvider.GetService<GetLogRecord>();
        private GetLogRecords GetLogRecords => serviceProvider.GetService<GetLogRecords>();

        public BlazorPageCore(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;

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
