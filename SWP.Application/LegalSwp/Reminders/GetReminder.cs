using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.LegalSwp.Reminders
{
    [TransientService]
    public class GetReminder
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetReminder(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Reminder Get(int id) => legalSwpManager.GetReminder(id, x => x);

    }
}
