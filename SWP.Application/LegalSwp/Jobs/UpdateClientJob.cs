using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Jobs
{
    [TransientService]
    public class UpdateClientJob
    {
        private readonly ILegalSwpManager legalSwpManager;
        public UpdateClientJob(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<ClientJob> Update(Request request)
        {
            var job = legalSwpManager.GetClientJob(request.Id, x => x);

            job.Name = request.Name;
            job.Description = request.Description;
            job.Priority = request.Priority;
            job.Active = request.Active;
            job.Updated = DateTime.Now;
            job.UpdatedBy = request.UpdatedBy;

            return legalSwpManager.UpdateClientJob(job);
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
