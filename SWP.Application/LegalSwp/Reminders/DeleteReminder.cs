using SWP.Domain.Infrastructure.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Reminders
{
    [TransientService]
    public class DeleteReminder
    {
        private readonly ILegalManager legalSwpManager;
        public DeleteReminder(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<int> Delete(int id) => legalSwpManager.DeleteReminder(id);

    }
}
