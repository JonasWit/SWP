using SWP.Domain.Infrastructure.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Clients
{
    [TransientService]
    public class DeleteClient : LegalActionsBase
    {
        public DeleteClient(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<int> Delete(int id) => _legalManager.DeleteClient(id);
        public Task<int> Delete(string profile) => _legalManager.DeleteProfileClients(profile);
    }
}
