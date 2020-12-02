using SWP.Domain.Infrastructure.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.PortalCustomers
{
    [TransientService]
    public class UpdateBillingRecord : PortalCustomersBase
    {
        public UpdateBillingRecord(IPortalManager portalManager) : base(portalManager)
        {
        }


    }
}
