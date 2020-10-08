using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.LegalSwp.ContactPeopleAdmin
{
    [TransientService]
    public class DeleteContactPerson
    {
        private readonly ILegalSwpManager legalSwpManager;
        public DeleteContactPerson(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;



    }
}
