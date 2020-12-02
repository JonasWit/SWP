using SWP.Domain.Infrastructure.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.PortalCustomers
{
    public abstract class PortalCustomersBase
    {
        protected readonly IPortalManager _portalManager;
        protected PortalCustomersBase(IPortalManager portalManager) => _portalManager = portalManager;
    }
}
