using SWP.Domain.Infrastructure.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.PortalCustomers
{
    [TransientService]
    public class LogActivity : PortalCustomersBase
    {
        public LogActivity(IPortalManager portalManager) : base(portalManager)
        {
        }


    }
}
