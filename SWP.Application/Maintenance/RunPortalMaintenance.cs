using SWP.Domain.Infrastructure.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.Maintenance
{
    [TransientService]
    public class RunPortalMaintenance
    {
        private readonly IPortalManager _portalManager;

        public RunPortalMaintenance(IPortalManager portalManager)
        {
            _portalManager = portalManager;
        }


        public Task<int> RunFullCleanup()
        {
            //todo: delete all clients accesses
            //todo: delete all cases accesses
            //todo: delete any possible billing data




            throw new NotImplementedException();
        }
    }
}
