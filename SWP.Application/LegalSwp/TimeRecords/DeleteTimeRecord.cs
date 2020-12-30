using SWP.Domain.Infrastructure.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.TimeRecords
{
    [TransientService]
    public class DeleteTimeRecord : LegalActionsBase
    {
        public DeleteTimeRecord(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<int> Delete(int id) => _legalManager.DeleteTimeRecord(id);
    }
}
