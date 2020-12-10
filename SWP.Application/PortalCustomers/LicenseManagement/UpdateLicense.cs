using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Models.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.PortalCustomers.LicenseManagement
{
    [TransientService]
    public class UpdateLicense : PortalManagerBase
    {
        public UpdateLicense(IPortalManager portalManager) : base(portalManager) { }

        public Task<UserLicense> Update(UserLicense license) => _portalManager.UpdateLicense(license);
    }
}
