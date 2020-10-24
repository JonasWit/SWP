using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.SWPLegal;

namespace SWP.Application.LegalSwp.Reminders
{
    [TransientService]
    public class GetReminder
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetReminder(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Reminder Get(int id) => legalSwpManager.GetReminder(id);
    }
}
