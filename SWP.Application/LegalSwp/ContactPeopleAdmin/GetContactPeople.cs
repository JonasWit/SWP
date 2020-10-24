using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.SWPLegal;
using System.Collections.Generic;

namespace SWP.Application.LegalSwp.ContactPeopleAdmin
{
    [TransientService]
    public class GetContactPeople
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetContactPeople(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public List<ClientContactPerson> GetClientContactPeople(int clientId) => legalSwpManager.GetContactPeopleForClient(clientId);

        public List<CaseContactPerson> GetCaseContactPeople(int caseId) => legalSwpManager.GetContactPeopleForCase(caseId);
    }
}
