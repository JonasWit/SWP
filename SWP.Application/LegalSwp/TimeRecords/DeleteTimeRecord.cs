using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.TimeRecords
{
    [TransientService]
    public class DeleteTimeRecord
    {
        private readonly ILegalSwpManager legalSwpManager;
        public DeleteTimeRecord(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<int> Delete(int id) => legalSwpManager.DeleteTimeRecord(id);
    }
}
