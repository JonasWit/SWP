using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SWP.Application.LegalSwp.AppDataAccess;
using SWP.Application.PortalCustomers.LicenseManagement;
using SWP.Domain.Enums;
using SWP.Domain.Infrastructure.Portal;
using SWP.UI.Pages.Applications.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.Models
{
    public class AppActiveUserManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _activeUserId;

        public IdentityUser User { get; set; }
        public List<Claim> Claims { get; set; } = new List<Claim>();
        public List<string> Roles { get; set; } = new List<string>();
        public List<IdentityUser> RelatedUsers { get; set; } = new List<IdentityUser>();
        public bool IsLocked => User.LockoutEnd != null;

        public List<LicenseViewModel> LicenseVms { get; set; } = new List<LicenseViewModel>();

        public string ProfileName => Claims.FirstOrDefault(x => x.Type == ClaimType.Profile.ToString())?.Value;
        public Claim ProfileClaim => Claims.FirstOrDefault(x => x.Type == ClaimType.Profile.ToString());

        public bool HasLegalLicense => LicenseVms.FirstOrDefault(x => x.Application == ApplicationType.LegalApplication) is not null ? true : false;
        public bool IsLegalLicenseActive => LicenseVms.FirstOrDefault(x => x.Application == ApplicationType.LegalApplication) is not null ? 
            LicenseVms.FirstOrDefault(x => x.Application == ApplicationType.LegalApplication).ValidTo >= DateTime.Now ? true : false : false;

        public bool IsRoot => Claims.Any(x => x.Type == ClaimType.Status.ToString() && x.Value == UserStatus.RootClient.ToString());
        public bool IsAdmin => Roles.Any(x => x.Equals(RoleType.Administrators.ToString()));

        public List<Claim> StatusClaims => Claims.Where(x => x.Type == ClaimType.Status.ToString()).ToList();
        public string UserName => User?.UserName;
        public string UserId => User?.Id;

        public List<int> CasesPermissions { get; set; } = new List<int>();
        public List<int> ClientsPermissions { get; set; } = new List<int>();
        public List<int> PanelsPermissions { get; set; } = new List<int>();

        public AppActiveUserManager(IServiceProvider serviceProvider, string activeUserId)
        {
            _serviceProvider = serviceProvider;
            _activeUserId = activeUserId;
        }

        public async Task UpdatePermissions()
        {
            using var scope = _serviceProvider.CreateScope();
            var getAccess = scope.ServiceProvider.GetRequiredService<GetAccess>();

            var clients = await getAccess.GetAccessToClient(_activeUserId);
            var cases = await getAccess.GetAccessToCase(_activeUserId);

            ClientsPermissions = clients.Select(x => x.ClientId).ToList();
            CasesPermissions = cases.Select(x => x.CaseId).ToList();
        }


        public async Task UpdateLicenses()
        {
            using var scope = _serviceProvider.CreateScope();
            var portalManager = scope.ServiceProvider.GetRequiredService<IPortalManager>();
            var getLicense = scope.ServiceProvider.GetRequiredService<GetLicense>();

            if (IsRoot)
            {
                LicenseVms = getLicense.GetAll(_activeUserId).Select(x => (LicenseViewModel)x).ToList();
            }
            else
            {
                var parentId = await portalManager.GetParentAccountId(User, ProfileClaim);

                if (!string.IsNullOrEmpty(parentId))
                {
                    LicenseVms = getLicense.GetAll(parentId).Select(x => (LicenseViewModel)x)?.ToList();
                }
            }
        }

        public async Task UpdateClaimsAndRoles()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                User = await userManager.FindByIdAsync(_activeUserId);
                Claims = await userManager.GetClaimsAsync(User) as List<Claim>;
                Roles = await userManager.GetRolesAsync(User) as List<string>;

                if (ProfileClaim is not null)
                {
                    RelatedUsers = await userManager.GetUsersForClaimAsync(ProfileClaim) as List<IdentityUser>;
                    RelatedUsers.RemoveAll(x => x.Id == _activeUserId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
