using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.Log
{
    [TransientService]
    public class DeleteLogRecord
    {
        private readonly ILogManager logManager;
        public DeleteLogRecord(ILogManager logManager) => this.logManager = logManager;

        public Task<int> Delete(int id) => logManager.DeleteLogRecord(id);

    }
}
