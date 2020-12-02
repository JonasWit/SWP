using SWP.Domain.Infrastructure;
using SWP.Domain.Models.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.Log
{
    [TransientService]
    public class CreateLogRecord
    {
        private readonly ILogManager logManager;
        public CreateLogRecord(ILogManager logManager) => this.logManager = logManager;

        public Task<LogRecord> Create(string userId, string message, string stack) =>
            logManager.CreateLogRecord(new LogRecord
            {
                UserId = userId,
                Message = message,
                StackTrace = stack,
                Created = DateTime.Now,
            });

        public Task<LogRecord> Create(Request request) =>
            logManager.CreateLogRecord(new LogRecord
            {
                UserId = request.UserId,
                Message = request.Message,
                StackTrace = request.StackTrace,
                Created = DateTime.Now,
            });

        public Task<LogRecord> CreateAutomationRecord(AutomationRequest request) =>
            logManager.CreateLogRecord(new LogRecord
            {
                UserId = "Automation",
                Message = request.Action,
                StackTrace = "Called by Scheduler",
                Created = DateTime.Now,
            });

        public class Request
        {
            [Required]
            public string UserId { get; set; }
            public string Message { get; set; }
            public string StackTrace { get; set; }
        }

        public class AutomationRequest
        {
            public string Action { get; set; }
            public DateTime TimeStamp { get; set; }
        }
    }
}
