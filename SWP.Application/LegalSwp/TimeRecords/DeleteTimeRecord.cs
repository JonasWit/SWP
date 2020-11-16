using SWP.Domain.Infrastructure.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.TimeRecords
{
    [TransientService]
    public class DeleteTimeRecord
    {
        private readonly ILegalManager legalSwpManager;
        public DeleteTimeRecord(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<int> Delete(int id) => legalSwpManager.DeleteTimeRecord(id);
    }
}
