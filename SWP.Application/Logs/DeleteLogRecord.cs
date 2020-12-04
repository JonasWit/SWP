using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.Logs
{
    [TransientService]
    public class DeleteLogRecord
    {
        private readonly ILogManager _logManager;
        public DeleteLogRecord(ILogManager logManager) => _logManager = logManager;

        public Task<int> Delete(int id) => _logManager.DeleteLogRecord(id);
    }
}
