using Microsoft.AspNetCore.Identity;
using SWP.Domain.Models.Portal;
using SWP.Domain.Models.Portal.Communication;
using SWP.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.Domain.Infrastructure.Portal
{
    public interface IPortalManager
    {
        BillingDetail GetBillingDetail(string userId);
        Task<BillingDetail> UpdateBillingDetail(BillingDetail details);
        Task<int> DeleteBillingDetail(string userId);

        List<UserLicense> GetLicenses(string userId);
        Task<UserLicense> CreateLicense(UserLicense license);
        Task<UserLicense> UpdateLicense(UserLicense license);
        Task<int> DeleteLicense(int id);
        Task<int> DeleteLicense(string userId);
        Task<int> DeleteLicenses(string userId);

        bool ClaimExists(string claimType, string claimValue);
        List<string> GetAllProfiles();
        List<string> GetRelatedUsers(string profile);
        List<UserLicense> GetLicensesForProfile(string profile);
        Task<string> GetParentAccountId(IdentityUser relatedUser, Claim profileClaim);

        Task<ManagerActionResult> ChangeProfileName(Claim oldProfile, string newProfile);
        Task<IdentityUser> GetUserByID(string id);
        Task<IdentityUser> GetUserByName(string name);
        Task<string> GetUserProfileByID(string id);
        Task<IList<IdentityUser>> GetUsersForProfile(Claim claim);

        ClientRequest GetRequestWithMessages(int requestId);
        List<ClientRequest> GetRequestsForClient(string userId);
        List<TResult> GetRequests<TResult>(Func<ClientRequest, TResult> selector, Func<ClientRequest, bool> predicate);
        List<ClientRequest> GetRequests();
        Task<int> CreateRequest(ClientRequest clientRequest);
        Task<int> CreateRequestMessage(ClientRequestMessage clientRequestMessage, int reuqestId);
        Task<int> DeleteRequest(int id);
        Task<int> DeleteRequest(string userId);
        Task<int> UpdateRequest(ClientRequest request);
    }
}
