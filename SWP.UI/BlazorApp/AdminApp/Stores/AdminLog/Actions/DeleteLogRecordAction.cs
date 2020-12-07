using SWP.Domain.Models.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.AdminLog.Actions
{
    public class DeleteLogRecordAction : IAction
    {
        public const string DeleteLogRecord = "DELETE_LOG_RECORD";
        public string Name => DeleteLogRecord;

        public Log Arg { get; set; }
    }
}
