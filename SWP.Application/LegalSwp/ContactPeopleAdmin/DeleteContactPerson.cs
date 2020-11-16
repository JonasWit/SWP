using SWP.Domain.Infrastructure.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.ContactPeopleAdmin
{
    [TransientService]
    public class DeleteContactPerson
    {
        private readonly ILegalManager legalSwpManager;
        public DeleteContactPerson(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<int> DeleteForClient(int id) => legalSwpManager.DeleteClientContactPerson(id);
        public Task<int> DeleteForCase(int id) => legalSwpManager.DeleteCaseContactPerson(id);
    }
}
