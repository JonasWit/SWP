using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Jobs
{
    [TransientService]
    public class UpdateCustomerJob
    {
        private readonly ILegalSwpManager legalSwpManager;
        public UpdateCustomerJob(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<int> Update(Request request)
        {
            var job = legalSwpManager.GetCustomerJob(request.Id, x => x);

            job.Name = request.Name;
            job.Description = request.Description;
            job.Priority = request.Priority;
            job.Active = request.Active;
            job.Updated = DateTime.Now;
            job.UpdatedBy = request.UpdatedBy;

            return legalSwpManager.UpdateCustomerJob(job);
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
