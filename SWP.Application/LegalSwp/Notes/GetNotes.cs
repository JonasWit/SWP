using SWP.Domain.Infrastructure.LegalApp;

namespace SWP.Application.LegalSwp.Notes
{
    [TransientService]
    public class GetNotes
    {
        private readonly ILegalManager legalSwpManager;
        public GetNotes(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;



    }
}
