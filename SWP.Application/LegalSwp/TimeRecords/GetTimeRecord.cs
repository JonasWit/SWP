using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;

namespace SWP.Application.LegalSwp.TimeRecords
{
    [TransientService]
    public class GetTimeRecord
    {
        private readonly ILegalManager legalSwpManager;
        public GetTimeRecord(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public TimeRecord Get(int id) => legalSwpManager.GetTimeRecord(id);
    }
}
