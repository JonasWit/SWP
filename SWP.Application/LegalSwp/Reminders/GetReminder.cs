using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;

namespace SWP.Application.LegalSwp.Reminders
{
    [TransientService]
    public class GetReminder
    {
        private readonly ILegalManager legalSwpManager;
        public GetReminder(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Reminder Get(int id) => legalSwpManager.GetReminder(id);
    }
}
