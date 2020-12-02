using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Jobs
{
    [TransientService]
    public class ArchiveJob
    {
        private readonly ILegalManager legalSwpManager;
        public ArchiveJob(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public int CountAllArchivedClients() => legalSwpManager.CountArchivedClients();

        public Task<ClientJob> ArchivizeClientJob(int jobId, string user) => legalSwpManager.ArchivizeClientJob(jobId, user);
        public Task<ClientJob> RecoverClientJob(int jobId, string user) => legalSwpManager.RecoverClientJob(jobId, user);
    }
}
