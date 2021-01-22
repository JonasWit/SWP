using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Utilities
{
    public struct PortalNames
    {
        public struct Policies
        {
            public const string BasicUser = nameof(BasicUser);
            public const string RootClient = nameof(RootClient);
            public const string RelatedAccount = nameof(RelatedAccount);
            public const string UnrelatedAccount = nameof(UnrelatedAccount);
            public const string LegalApplication = nameof(LegalApplication);
            public const string AuthenticatedUser = nameof(AuthenticatedUser);
        }

        public struct Roles
        {
            public const string Users = nameof(Users);
            public const string Administrators = nameof(Administrators);
        }

        public struct InternalEmail
        {
            public const string Office = "biuro@systemywp.pl";
        }
    }
}
