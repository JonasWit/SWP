using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;

namespace SWP.Application.LegalSwp.Reminders
{
    [TransientService]
    public class GetReminder : LegalActionsBase
    {
        public GetReminder(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Reminder Get(int id) => _legalManager.GetReminder(id);
    }
}
