using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;

namespace SWP.Application.LegalSwp.ContactPeopleAdmin
{
    [TransientService]
    public class GetContactPerson
    {
        private readonly ILegalManager legalSwpManager;
        public GetContactPerson(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public ClientContactPerson GetForClient(int id) => legalSwpManager.GetClientContactPerson(id);
        public CaseContactPerson GetForCase(int id) => legalSwpManager.GetCaseContactPerson(id);
    }
}
