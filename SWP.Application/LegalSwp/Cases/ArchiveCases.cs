using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Cases
{
    [TransientService]
    public class ArchiveCases
    {
        private readonly ILegalSwpManager legalSwpManager;
        public ArchiveCases(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public int CountAllArchivedCases() => legalSwpManager.CountArchivedCases();

        public Task<int> ArchivizeCase(int caseId, string user) => legalSwpManager.ArchivizeCase(caseId, user);

        public Case RecoverCase(int caseId, string user) => legalSwpManager.RecoverCase(caseId, user);
    }
}
