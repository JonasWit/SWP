using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.LegalSwp.ContactPeopleAdmin
{
    [TransientService]
    public class GetContactPeople
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetContactPeople(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;



    }
}
