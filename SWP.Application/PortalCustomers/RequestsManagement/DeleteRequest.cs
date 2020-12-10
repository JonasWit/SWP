using SWP.Domain.Infrastructure.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.PortalCustomers.RequestsManagement
{
    [TransientService]
    public class DeleteRequest : PortalManagerBase
    {
        public DeleteRequest(IPortalManager portalManager) : base(portalManager)
        {
        }

        public Task<int> Delete(int id) => _portalManager.DeleteRequest(id);

        public Task<int> Delete(string userId) => _portalManager.DeleteRequest(userId);
    }
}
