﻿using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.ContactPeopleAdmin
{
    [TransientService]
    public class DeleteContactPerson
    {
        private readonly ILegalSwpManager legalSwpManager;
        public DeleteContactPerson(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<int> DeleteForClient(int id) => legalSwpManager.DeleteClientContactPerson(id);
        public Task<int> DeleteForCase(int id) => legalSwpManager.DeleteCaseContactPerson(id);
    }
}