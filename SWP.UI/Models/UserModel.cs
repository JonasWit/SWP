using Microsoft.AspNetCore.Identity;
using SWP.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SWP.UI.Models
{
    public class UserModel
    {
        public IdentityUser User { get; set; }
        public List<Claim> Claims { get; set; } = new List<Claim>();
        public List<string> Roles { get; set; } = new List<string>();
        public List<IdentityUser> RelatedUsers { get; set; } = new List<IdentityUser>();
        public bool IsLocked => User.LockoutEnd != null;

        public string Profile => Claims.FirstOrDefault(x => x.Type == ClaimType.Profile.ToString())?.Value;
        public bool RootClient => Claims.Any(x => x.Type == ClaimType.Status.ToString() && x.Value == UserStatus.RootClient.ToString());
        public List<Claim> StatusClaims => Claims.Where(x => x.Type == ClaimType.Status.ToString()).ToList();
        public Claim ProfileClaim => Claims.FirstOrDefault(x => x.Type == ClaimType.Profile.ToString());
        public string UserName => User?.UserName;
    }
}
