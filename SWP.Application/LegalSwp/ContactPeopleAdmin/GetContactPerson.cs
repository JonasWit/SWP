using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;

namespace SWP.Application.LegalSwp.ContactPeopleAdmin
{
    [TransientService]
    public class GetContactPerson : LegalActionsBase
    {
        public GetContactPerson(ILegalManager legalManager) : base(legalManager)
        {
        }

        public ClientContactPerson GetForClient(int id) => _legalManager.GetClientContactPerson(id);
        public CaseContactPerson GetForCase(int id) => _legalManager.GetCaseContactPerson(id);
    }
}
