using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Jobs
{
    [TransientService]
    public class ArchiveJob : LegalActionsBase
    {
        public ArchiveJob(ILegalManager legalManager) : base(legalManager)
        {
        }

        public int CountAllArchivedClients() => _legalManager.CountArchivedClients();

        public Task<ClientJob> ArchivizeClientJob(int jobId, string user) => _legalManager.ArchivizeClientJob(jobId, user);
        public Task<ClientJob> RecoverClientJob(int jobId, string user) => _legalManager.RecoverClientJob(jobId, user);
    }
}
