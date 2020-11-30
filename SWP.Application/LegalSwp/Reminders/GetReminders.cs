using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.SWPLegal;
using System.Collections.Generic;

namespace SWP.Application.LegalSwp.Reminders
{
    [TransientService]
    public class GetReminders
    {
        private readonly ILegalManager legalSwpManager;
        public GetReminders(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public List<Reminder> Get(string profileClaim) => legalSwpManager.GetReminders(profileClaim);
        public List<Reminder> GetUpcoming(string profileClaim) => legalSwpManager.GetUpcomingReminders(profileClaim);
        public List<Reminder> Get(int clientId) => legalSwpManager.GetRemindersForClient(clientId);
    }
}
