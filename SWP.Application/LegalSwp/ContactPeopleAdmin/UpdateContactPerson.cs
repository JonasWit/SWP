using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.LegalSwp.ContactPeopleAdmin
{
    [TransientService]
    public class UpdateContactPerson
    {
        private readonly ILegalSwpManager legalSwpManager;
        public UpdateContactPerson(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;




    }
}
