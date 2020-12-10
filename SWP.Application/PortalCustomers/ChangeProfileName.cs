using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Utilities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.Application.PortalCustomers
{
    [TransientService]
    public class ChangeProfileName : PortalCustomersBase
    {
        private readonly IPortalManager _portalManager;

        public ChangeProfileName(IPortalManager portalManager) : base(portalManager)
        {
            _portalManager = portalManager;
        }

        public Task<ManagerActionResult> Change(Claim oldProfile, string newProfile) => _portalManager.ChangeProfileName(oldProfile, newProfile);
    }
}
