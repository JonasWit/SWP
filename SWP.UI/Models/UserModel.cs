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
        public string Profile => Claims.FirstOrDefault(x => x.Type == ClaimType.Profile.ToString())?.Value;
        public string UserName => User?.UserName;
    }
}
