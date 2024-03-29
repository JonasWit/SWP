﻿using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Jobs
{
    [TransientService]
    public class UpdateClientJob : LegalActionsBase
    {
        public UpdateClientJob(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<ClientJob> Update(Request request)
        {
            var job = _legalManager.GetClientJob(request.Id);

            job.Name = request.Name;
            job.Description = request.Description;
            job.Priority = request.Priority;
            job.Active = request.Active;
            job.Updated = DateTime.Now;
            job.UpdatedBy = request.UpdatedBy;

            return _legalManager.UpdateClientJob(job);
        }

        public class Request
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public bool Active { get; set; }
            public int Priority { get; set; }

            public DateTime Updated { get; set; }
            public string UpdatedBy { get; set; }
        }


    }
}
