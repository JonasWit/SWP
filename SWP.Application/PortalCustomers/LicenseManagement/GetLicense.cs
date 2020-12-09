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
    public class GetLicense : PortalCustomersBase
    {
        public GetLicense(IPortalManager portalManager) : base(portalManager) { }

        public List<UserLicense> GetAll(string userId) => _portalManager.GetLicenses(userId);
    }
}
