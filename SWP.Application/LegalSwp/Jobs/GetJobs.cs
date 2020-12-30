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
    public class GetJobs : LegalActionsBase
    {
        public GetJobs(ILegalManager legalManager) : base(legalManager)
        {
        }

        public List<ClientJob> GetClientJobs(int clientId) => _legalManager.GetClientJobs(clientId);
    }
}
