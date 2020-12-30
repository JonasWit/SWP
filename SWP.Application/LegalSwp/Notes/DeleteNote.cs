using SWP.Domain.Infrastructure.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Notes
{
    [TransientService]
    public class DeleteNote : LegalActionsBase
    {
        public DeleteNote(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<int> Delete(int id) => _legalManager.DeleteNote(id);
    }
}
