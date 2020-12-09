using Microsoft.AspNetCore.Identity;
using SWP.Domain.Enums;
using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Models.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SWP.DataBase.Managers
{
    public class IdentityExtendedManager : DataManagerBase, IIdentityExtendedManager
    {
        private readonly UserManager<IdentityUser> _userManager;

        public IdentityExtendedManager(AppContext context, UserManager<IdentityUser> userManager) : base(context) 
        {
            _userManager = userManager;
        }

        public bool ClaimExists(string claimType, string claimValue)
        {
            if (_context.UserClaims.Any(x => x.ClaimType == claimType && x.ClaimValue == claimValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<string> GetAllProfiles() => 
            _context.UserClaims.Where(x => x.ClaimType == ClaimType.Profile.ToString()).Select(x => x.ClaimValue).Distinct().ToList();

        public List<UserLicense> GetLicensesForProfile(string profile)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetParentAccountId(IdentityUser relatedUser, Claim profileClaim)
        {
            var profileUsers = await _userManager.GetUsersForClaimAsync(profileClaim) as List<IdentityUser>;

            foreach (var profileUser in profileUsers)
            {
                var claims = await _userManager.GetClaimsAsync(profileUser) as List<Claim>;
                if (claims.Any(x => x.Type == ClaimType.Status.ToString() && x.Value == UserStatus.RootClient.ToString()))
                {
                    return profileUser.Id;
                }
            }

            return null;
        }

        public List<string> GetRelatedUsers(string profile)
        {
            throw new NotImplementedException();
        }
    }
}
