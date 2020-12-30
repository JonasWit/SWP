using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System;
using System.Collections.Generic;

namespace SWP.Application.LegalSwp.Reminders
{
    [TransientService]
    public class GetReminders : LegalActionsBase
    {
        public GetReminders(ILegalManager legalManager) : base(legalManager)
        {
        }

        public List<Reminder> Get(string profileClaim) => _legalManager.GetReminders(profileClaim);
        public List<Reminder> GetUpcoming(string profileClaim) => _legalManager.GetUpcomingReminders(profileClaim);
        public List<Reminder> GetUpcoming(string profileClaim, DateTime date) => _legalManager.GetUpcomingReminders(profileClaim, date);
        public List<Reminder> Get(int clientId) => _legalManager.GetRemindersForClient(clientId);
    }
}
