using SWP.Domain.Infrastructure.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.PortalCustomers
{
    [TransientService]
    public class ClearCustomerRelatedData : PortalCustomersBase
    {
        public ClearCustomerRelatedData(IPortalManager portalManager) : base(portalManager)
        {
        }

        public Task<int> CleanUp(string userId) => _portalManager.ClearCustomerData(userId);



    }
}
