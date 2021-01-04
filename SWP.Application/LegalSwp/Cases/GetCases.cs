using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System.Collections.Generic;

namespace SWP.Application.LegalSwp.Cases
{
    [TransientService]
    public class GetCases : LegalActionsBase
    {
        public GetCases(ILegalManager legalManager) : base(legalManager)
        {
        }

        public int CountDeadlineRemindersPerCase(int id) => _legalManager.CountDeadlineRemindersPerCase(id);
        public int CountRemindersPerCase(int id) => _legalManager.CountRemindersPerCase(id);
        public int CountNotesPerCase(int id) => _legalManager.CountNotesPerCase(id);
        public List<Case> GetArchivedCases(int clientId) => _legalManager.GetArchivedCases(clientId);
        public List<Case> GetCasesForClient(int clientId) => _legalManager.GetCasesForClient(clientId);
        public List<Case> GetCasesForClient(int clientId, List<int> allowedIds) => _legalManager.GetCasesForClient(clientId, allowedIds);
    }
}
