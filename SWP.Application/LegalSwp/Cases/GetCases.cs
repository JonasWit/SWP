using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.SWPLegal;
using System.Collections.Generic;

namespace SWP.Application.LegalSwp.Cases
{
    [TransientService]
    public class GetCases
    {
        private readonly ILegalManager legalSwpManager;
        public GetCases(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public int CountDeadlineRemindersPerCase(int id) => legalSwpManager.CountDeadlineRemindersPerCase(id);
        public int CountRemindersPerCase(int id) => legalSwpManager.CountRemindersPerCase(id);
        public int CountNotesPerCase(int id) => legalSwpManager.CountNotesPerCase(id);
        public List<Case> GetArchivedCases(int clientId) => legalSwpManager.GetArchivedCases(clientId);
    }
}
