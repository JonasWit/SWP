using SWP.Domain.Infrastructure.LegalApp;

namespace SWP.Application.LegalSwp.Notes
{
    [TransientService]
    public class GetNotes
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetNotes(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;



    }
}
