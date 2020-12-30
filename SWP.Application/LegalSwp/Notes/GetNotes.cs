using SWP.Domain.Infrastructure.LegalApp;

namespace SWP.Application.LegalSwp.Notes
{
    [TransientService]
    public class GetNotes : LegalActionsBase
    {
        public GetNotes(ILegalManager legalManager) : base(legalManager)
        {
        }
    }
}
