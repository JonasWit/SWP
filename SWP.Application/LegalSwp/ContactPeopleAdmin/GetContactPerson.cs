using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Text;

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
