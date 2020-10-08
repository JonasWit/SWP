using SWP.Domain.Infrastructure;
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


    }
}
