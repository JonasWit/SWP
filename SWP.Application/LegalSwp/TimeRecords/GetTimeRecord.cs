using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;

namespace SWP.Application.LegalSwp.TimeRecords
{
    [TransientService]
    public class GetTimeRecord : LegalActionsBase
    {
        public GetTimeRecord(ILegalManager legalManager) : base(legalManager)
        {
        }

        public TimeRecord Get(int id) => _legalManager.GetTimeRecord(id);
    }
}
