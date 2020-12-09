using SWP.Domain.Models.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Domain.Infrastructure.Portal
{
    public interface IIdentityExtendedManager
    {
        bool ClaimExists(string claimType, string claimValue);
        List<string> GetAllProfiles();
        List<string> GetRelatedUsers(string profile);
        List<UserLicense> GetLicensesForProfile(string profile);
        string GetParentAccountId(string relaredId);    
    }
}
