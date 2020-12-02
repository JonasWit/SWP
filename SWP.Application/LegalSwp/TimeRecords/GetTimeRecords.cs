using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System.Collections.Generic;

namespace SWP.Application.LegalSwp.TimeRecords
{
    [TransientService]
    public class GetTimeRecords
    {
        private readonly ILegalManager legalSwpManager;
        public GetTimeRecords(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public List<TimeRecord> Get(int clientId) => legalSwpManager.GetTimeRecords(clientId);
    }
}
