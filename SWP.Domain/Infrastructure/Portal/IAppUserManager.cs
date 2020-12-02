using Microsoft.AspNetCore.Identity;
using SWP.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.Domain.Infrastructure.Portal
{
    public interface IAppUserManager
    {
        Task<ManagerActionResult> ChangeProfileName(Claim oldProfile, string newProfile);
        Task<IdentityUser> GetUserByID(string id);
        Task<IdentityUser> GetUserByName(string name);
        Task<string> GetUserProfileByID(string id);
        Task<IList<IdentityUser>> GetUsersForProfile(Claim claim);
    }
}