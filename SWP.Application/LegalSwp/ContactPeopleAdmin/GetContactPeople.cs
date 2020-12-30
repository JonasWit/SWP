using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System.Collections.Generic;

namespace SWP.Application.LegalSwp.ContactPeopleAdmin
{
    [TransientService]
    public class GetContactPeople : LegalActionsBase
    {
        public GetContactPeople(ILegalManager legalManager) : base(legalManager)
        {
        }

        public List<ClientContactPerson> GetClientContactPeople(int clientId) => _legalManager.GetContactPeopleForClient(clientId);

        public List<CaseContactPerson> GetCaseContactPeople(int caseId) => _legalManager.GetContactPeopleForCase(caseId);
    }
}
