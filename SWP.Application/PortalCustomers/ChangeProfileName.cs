using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Utilities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.Application.PortalCustomers
{
    [TransientService]
    public class ChangeProfileName : PortalManagerBase
    {
        public ChangeProfileName(IPortalManager portalManager) : base(portalManager)
        {
        }

        public Task<ManagerActionResult> Change(Claim oldProfile, string newProfile) => _portalManager.ChangeProfileName(oldProfile, newProfile);
    }
}
