using SWP.Domain.Infrastructure.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.PortalCustomers
{
    [TransientService]
    public class DeleteBillingRecord : PortalCustomersBase
    {
        public DeleteBillingRecord(IPortalManager portalManager) : base(portalManager) { }

        public Task<int> DeleteBillingDetail(string userId) => _portalManager.DeleteBillingDetail(userId);

    }
}
