using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.SWPLegal;

namespace SWP.Application.LegalSwp.Cases
{
    [TransientService]
    public class GetCase
    {
        private readonly ILegalManager legalSwpManager;
        public GetCase(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Case Get(int id) => legalSwpManager.GetCase(id);
        public string GetCaseParentName(int id) => legalSwpManager.GetCaseParentName(id);
        public string GetCaseName(int id) => legalSwpManager.GetCaseName(id);
    }
}
