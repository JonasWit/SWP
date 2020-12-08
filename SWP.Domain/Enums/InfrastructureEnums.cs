using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Domain.Enums
{
    public enum ApplicationType
    { 
        LegalSwp = 0,
    }

    public enum UserStatus
    { 
        RootClient = 0,
        Default = 1,
        Undefined = 2
    }

    public enum RoleType
    {
        Administrators = 0,
        Users = 1,
        All = -1
    }

    public enum ClaimType
    {
        Application = 0,
        Profile = 1,
        Status = 2,
    }

    public enum ApplicationPolicy
    {
        LegalApplication = 0,
    }

    public enum UserDataClaims
    {
        ProfileName = 0,
        ProfileSurname = 1,
        ProfileAddress = 2,
        ProfileAddressCorrespondence = 3,
        City = 4,
        Vivodership = 5,
        Country = 6,
        PostCode = 7,
        CompanyFullName = 8,
        NIP = 9,
        REGON = 10,
        PESEL = 11,
        KRS = 12,
    }
}
