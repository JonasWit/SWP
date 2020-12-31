using SWP.Domain.Infrastructure.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.PortalCustomers
{
    [TransientService]
    public class GetAllProfiles : PortalManagerBase
    {
        public GetAllProfiles(IPortalManager portalManager) : base(portalManager)
        {
        }

        public List<string> Get() => _portalManager.GetAllProfiles();
    }
}
