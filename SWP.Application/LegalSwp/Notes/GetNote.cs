using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;

namespace SWP.Application.LegalSwp.Notes
{
    [TransientService]
    public class GetNote : LegalActionsBase
    {
        public GetNote(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Note Get(int id) => _legalManager.GetNote(id);
    }
}
