using SWP.Domain.Infrastructure.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Reminders
{
    [TransientService]
    public class DeleteReminder : LegalActionsBase
    {
        public DeleteReminder(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<int> Delete(int id) => _legalManager.DeleteReminder(id);

    }
}
