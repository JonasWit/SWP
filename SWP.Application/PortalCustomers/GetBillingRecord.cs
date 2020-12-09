using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Models.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.PortalCustomers
{
    [TransientService]
    public class GetBillingRecord : PortalCustomersBase
    {
        public GetBillingRecord(IPortalManager portalManager) : base(portalManager) { }

        public BillingDetail GetDetails(string userId) => _portalManager.GetBillingDetail(userId);
    }
}
