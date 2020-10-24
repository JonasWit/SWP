using SWP.Domain.Infrastructure.LegalApp;

namespace SWP.Application.LegalSwp.Cases
{
    [TransientService]
    public class GetCases
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetCases(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public int CountDeadlineRemindersPerCase(int id) => legalSwpManager.CountDeadlineRemindersPerCase(id);
        public int CountRemindersPerCase(int id) => legalSwpManager.CountRemindersPerCase(id);
        public int CountNotesPerCase(int id) => legalSwpManager.CountNotesPerCase(id);
    }
}
