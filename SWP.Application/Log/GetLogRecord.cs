using SWP.Domain.Infrastructure;
using SWP.Domain.Models.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.Log
{
    [TransientService]
    public class GetLogRecord
    {
        private readonly ILogManager logManager;
        public GetLogRecord(ILogManager logManager) => this.logManager = logManager;

        public LogRecord Get(int id) => logManager.GetLogRecord(id);
    }
}
