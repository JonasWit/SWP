using SWP.Domain.Infrastructure;
using SWP.Domain.Models.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.Log
{
    [TransientService]
    public class GetLogRecords
    {
        private readonly ILogManager logManager;
        public GetLogRecords(ILogManager logManager) => this.logManager = logManager;

        public List<LogRecord> GetRecords() => logManager.GetGetLogRecords();

        public List<LogRecord> GetRecordsWitohutStacks() => 
            logManager.GetLogRecordsWithSpecificData(x => new LogRecord 
            { 
                Id = x.Id, 
                Created = x.Created,
                Message = x.Message,
                UserId = x.UserId 
            });
    }
}
