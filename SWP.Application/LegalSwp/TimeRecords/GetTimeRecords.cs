using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System.Collections.Generic;

namespace SWP.Application.LegalSwp.TimeRecords
{
    [TransientService]
    public class GetTimeRecords : LegalActionsBase
    {
        public GetTimeRecords(ILegalManager legalManager) : base(legalManager)
        {
        }

        public List<TimeRecord> Get(int clientId) => _legalManager.GetTimeRecords(clientId);
    }
}
