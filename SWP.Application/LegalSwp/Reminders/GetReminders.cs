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
        public List<Reminder> Get(string profileClaim, List<int> allowedCases) => _legalManager.GetReminders(profileClaim, allowedCases);

        public List<Reminder> GetUpcoming(string profileClaim, DateTime date) => _legalManager.GetUpcomingReminders(profileClaim, date);
        public List<Reminder> GetUpcoming(string profileClaim, DateTime date, List<int> allowedCases) => _legalManager.GetUpcomingReminders(profileClaim, date, allowedCases);
        public List<Reminder> GetUpcoming(int clientId, DateTime date) => _legalManager.GetUpcomingReminders(clientId, date);
        public List<Reminder> GetUpcoming(int clientId, DateTime date, List<int> allowedCases) => _legalManager.GetUpcomingReminders(clientId, date, allowedCases);

        public List<Reminder> Get(int clientId) => _legalManager.GetRemindersForClient(clientId);
        public List<Reminder> Get(int clientId, List<int> allowedCases) => _legalManager.GetRemindersForClient(clientId, allowedCases);
    }
}
