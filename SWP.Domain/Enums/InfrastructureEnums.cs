﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Domain.Enums
{
    public enum LegalAppPanels
    {
        Clients = 0,
        Calendar = 1,
        Cases = 2,
        MyApp = 3,
        ErrorPage = 4,
        Finance = 5,
        Productivity = 6,
        ClientJobs = 7,
        Archive = 8,
        ClientDetails = 9,
        UserManager = 10,
        Info = 11
    }

    public enum LegalAppActions
    {
        CanArchive = 0,
        CanDelete = 1
    }

    public enum RequestReason
    {
        Query = 0,
        PurchaseLicense = 1,
        ModifyLicense = 2,
        ExtendLicense = 3,
        RequestDemo = 4,
        TechnicalIssue = 5
    }

    public enum RequestStatus
    {
        WaitingForAnswer = 0,
        Answered = 1,
        Solved = 2
    }

    public enum ApplicationType
    {
        LegalApplication = 0,
        NoApp = 1,
    }

    public enum UserStatus
    { 
        RootClient = 0,
        Default = 1,
        Undefined = 2,
        RelatedUser = 3
    }

    public enum LicenseType
    {
        Trial = 0,
        Regular = 1
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
