using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.LegalSwp.Reminders
{
    [TransientService]
    public class GetReminders
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetReminders(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public List<Reminder> Get(string profileClaim) => legalSwpManager.GetReminders(profileClaim);
        public List<Reminder> Get(int ClientId) => legalSwpManager.GetRemindersForClient(ClientId);
    }
}
