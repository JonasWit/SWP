using SWP.Domain.Infrastructure.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Clients
{
    [TransientService]
    public class DeleteClient
    {
        private readonly ILegalManager legalSwpManager;
        public DeleteClient(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<int> Delete(int id) => legalSwpManager.DeleteClient(id);
        public Task<int> Delete(string profile) => legalSwpManager.DeleteProfileClients(profile);
    }
}
