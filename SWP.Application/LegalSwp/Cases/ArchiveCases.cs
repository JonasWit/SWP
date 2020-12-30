using SWP.Domain.Infrastructure.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Cases
{
    [TransientService]
    public class ArchiveCases : LegalActionsBase
    {
        public ArchiveCases(ILegalManager legalManager) : base(legalManager)
        {
        }

        public int CountAllArchivedCases() => _legalManager.CountArchivedCases();

        public Task<int> ArchivizeCase(int caseId, string user) => _legalManager.ArchivizeCase(caseId, user);

        public Task<int> RecoverCase(int caseId, string user) => _legalManager.RecoverCase(caseId, user);
    }
}
