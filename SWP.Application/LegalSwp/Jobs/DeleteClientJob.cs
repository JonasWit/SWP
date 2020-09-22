﻿using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Jobs
{
    [TransientService]
    public class DeleteClientJob
    {
        private readonly ILegalSwpManager legalSwpManager;
        public DeleteClientJob(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<int> Delete(int id) => legalSwpManager.DeleteClientJob(id);



    }
}