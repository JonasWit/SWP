﻿using SWP.Domain.Enums;
using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Models.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.DataBase.Managers
{
    public class IdentityExtendedManager : DataManagerBase, IIdentityExtendedManager
    {
        public IdentityExtendedManager(AppContext context) : base(context) { }

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

        public string GetParentAccountId(string relaredId)
        {
            throw new NotImplementedException();
        }

        public List<string> GetRelatedUsers(string profile)
        {
            throw new NotImplementedException();
        }
    }
}
