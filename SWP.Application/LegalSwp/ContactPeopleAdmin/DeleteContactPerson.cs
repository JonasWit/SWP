using SWP.Domain.Infrastructure.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.ContactPeopleAdmin
{
    [TransientService]
    public class DeleteContactPerson : LegalActionsBase
    {
        public DeleteContactPerson(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<int> DeleteForClient(int id) => _legalManager.DeleteClientContactPerson(id);
        public Task<int> DeleteForCase(int id) => _legalManager.DeleteCaseContactPerson(id);
    }
}
