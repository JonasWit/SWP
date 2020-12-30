using SWP.Domain.Infrastructure.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Cases
{
    [TransientService]
    public class DeleteCase : LegalActionsBase
    {
        public DeleteCase(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<int> Delete(int id) => _legalManager.DeleteCase(id);




    }
}
