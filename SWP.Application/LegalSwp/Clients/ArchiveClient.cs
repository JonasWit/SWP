using SWP.Domain.Infrastructure.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Clients
{
    [TransientService]
    public class ArchiveClient : LegalActionsBase
    {
        public ArchiveClient(ILegalManager legalManager) : base(legalManager)
        {
        }

        public int CountAllArchivedClients() => _legalManager.CountArchivedClients();

        public Task<int> ArchivizeClient(int clientId, string user) => _legalManager.ArchivizeClient(clientId, user);
        public Task<int> RecoverClient(int clientId, string user) => _legalManager.RecoverClient(clientId, user);
    }
}
