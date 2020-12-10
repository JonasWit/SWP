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
    public class UpdateBillingRecord : PortalManagerBase
    {
        public UpdateBillingRecord(IPortalManager portalManager) : base(portalManager) { }

        public Task<BillingDetail> Update(BillingDetail billingDetail) => _portalManager.UpdateBillingDetail(billingDetail);

    }
}
