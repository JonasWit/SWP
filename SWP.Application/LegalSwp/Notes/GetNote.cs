using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.SWPLegal;

namespace SWP.Application.LegalSwp.Notes
{
    [TransientService]
    public class GetNote
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetNote(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;
        public Note Get(int id) => legalSwpManager.GetNote(id);
    }
}
