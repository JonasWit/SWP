using Microsoft.AspNetCore.Identity;
using SWP.Domain.Enums;
using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.DataBase.Managers
{
    public class AppUserManager : DataManagerBase, IAppUserManager
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AppUserManager(UserManager<IdentityUser> userManager, AppContext context) : base(context)
        {
            _userManager = userManager;
        }

        public Task<IList<IdentityUser>> GetUsersForProfile(Claim claim) => _userManager.GetUsersForClaimAsync(claim);

        public Task<IdentityUser> GetUserByID(string id) => _userManager.FindByIdAsync(id);

        public Task<IdentityUser> GetUserByName(string name) => _userManager.FindByNameAsync(name);

        public async Task<string> GetUserProfileByID(string userId)
        {
            var claims = await _userManager.GetClaimsAsync(await GetUserByID(userId)) as List<Claim>;
            var profileClaim = claims.FirstOrDefault(x => x.Type == ClaimType.Profile.ToString());
            return profileClaim.Value;
        }

        public async Task<ManagerActionResult> ChangeProfileName(Claim oldProfile, string newProfile)
        {
            try
            {
                var users = await GetUsersForProfile(oldProfile) as List<IdentityUser>;

                foreach (var user in users)
                {
                    var removeResult = await _userManager.RemoveClaimAsync(user, oldProfile);

                    if (removeResult.Succeeded)
                    {
                        var newClaim = new Claim(ClaimType.Profile.ToString(), newProfile.Trim());
                        var addResult = await _userManager.AddClaimAsync(user, newClaim);

                        if (!addResult.Succeeded)
                        {
                            throw new Exception($"Error when adding new Profile for user {user.UserName}, Issues: {string.Join("; ", addResult.Errors.Select(x => x.Description).ToList())}");
                        }
                    }
                    else
                    {
                        throw new Exception($"Error when removing old Profile for user {user.UserName}, Issues: { string.Join("; ", removeResult.Errors.Select(x => x.Description).ToList()) }");
                    }
                }

                var clientsToUpdate = _context.Clients.Where(x => x.ProfileClaim == oldProfile.Value).ToList();

                foreach (var clinet in clientsToUpdate)
                {
                    clinet.ProfileClaim = newProfile;
                }

                _context.UpdateRange(clientsToUpdate);
                await _context.SaveChangesAsync();
                return null;
            }
            catch (ManagerActionResult ex)
            {
                return ex;
            }
            finally
            { 
         
                //todo: check and cleanup all root profiles or incorrect profiles
            }
        }
    }
}
