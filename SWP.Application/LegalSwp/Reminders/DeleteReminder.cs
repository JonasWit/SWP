using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Reminders
{
    [TransientService]
    public class DeleteReminder
    {
        private readonly ILegalSwpManager legalSwpManager;
        public DeleteReminder(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<int> Delete(int id) => legalSwpManager.DeleteReminder(id);

    }
}
