using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;

namespace SWP.Application.LegalSwp.Cases
{
    [TransientService]
    public class GetCase : LegalActionsBase
    {
        public GetCase(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Case Get(int id) => _legalManager.GetCase(id);
        public string GetCaseParentName(int id) => _legalManager.GetCaseParentName(id);
        public string GetCaseName(int id) => _legalManager.GetCaseName(id);
    }
}
