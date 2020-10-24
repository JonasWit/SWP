using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.SWPLegal;

namespace SWP.Application.LegalSwp.ContactPeopleAdmin
{
    [TransientService]
    public class GetContactPerson
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetContactPerson(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public ClientContactPerson GetForClient(int id) => legalSwpManager.GetClientContactPerson(id);
        public CaseContactPerson GetForCase(int id) => legalSwpManager.GetCaseContactPerson(id);
    }
}
