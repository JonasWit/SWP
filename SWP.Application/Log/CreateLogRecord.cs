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

        public Task<LogRecord> Create(Request request) =>
            logManager.CreateLogRecord(new LogRecord
            {
                UserId = request.UserId,
                Message = request.Message,
                StackTrace = request.StackTrace,
                Created = DateTime.Now,
            });

        public class Request
        {
            [Required]
            public string UserId { get; set; }
            public string Message { get; set; }
            public string StackTrace { get; set; }
        }
    }
}
