using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.SWPLegal;

namespace SWP.Application.LegalSwp.TimeRecords
{
    [TransientService]
    public class GetTimeRecord
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetTimeRecord(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public TimeRecord Get(int id) => legalSwpManager.GetTimeRecord(id);
    }
}
