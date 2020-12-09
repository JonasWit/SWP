using SWP.Domain.Infrastructure.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.PortalCustomers.LicenseManagement
{
    [TransientService]
    public class DeleteLicense : PortalCustomersBase
    {
        public DeleteLicense(IPortalManager portalManager) : base(portalManager) { }

        public Task<int> Delete(int id) => _portalManager.DeleteLicense(id);
    }
}
