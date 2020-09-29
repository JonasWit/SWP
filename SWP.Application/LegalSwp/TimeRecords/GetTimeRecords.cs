using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.LegalSwp.TimeRecords
{
    [TransientService]
    public class GetTimeRecords
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetTimeRecords(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public List<TimeRecord> Get(int clientId) => legalSwpManager.GetTimeRecords(clientId);
    }
}
