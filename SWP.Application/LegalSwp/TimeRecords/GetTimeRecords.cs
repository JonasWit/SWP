using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.SWPLegal;
using System.Collections.Generic;

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
