using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Jobs
{
    [TransientService]
    public class GetJobs
    {
        private readonly ILegalManager legalSwpManager;
        public GetJobs(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public List<ClientJob> GetClientJobs(int clientId) => legalSwpManager.GetClientJobs(clientId);
    }
}
