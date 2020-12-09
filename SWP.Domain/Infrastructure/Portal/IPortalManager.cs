using SWP.Domain.Models.Portal;
using System.Collections.Generic;
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
    }
}
