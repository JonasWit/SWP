using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Domain.Enums
{
    public enum ApplicationType
    { 
        LegalSwp = 0,
        MedicalSwp = 1,
    }

    public enum UserStatus
    { 
        RootClient = 0
    }

    public enum RoleType
    {
        Administrators = 0,
        Users = 1,
    }

    public enum ClaimType
    {
        Application = 0,
        Profile = 1,
        Status = 2,
    }


}
