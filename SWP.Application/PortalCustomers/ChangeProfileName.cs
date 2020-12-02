using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Utilities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.Application.PortalCustomers
{
    [TransientService]
    public class ChangeProfileName : PortalCustomersBase
    {
        private readonly IAppUserManager _appUserManager;

        public ChangeProfileName(IPortalManager portalManager, IAppUserManager appUserManager) : base(portalManager)
        {
            _appUserManager = appUserManager;
        }

        public Task<ManagerActionResult> Change(Claim oldProfile, string newProfile) => _appUserManager.ChangeProfileName(oldProfile, newProfile);
    }
}
