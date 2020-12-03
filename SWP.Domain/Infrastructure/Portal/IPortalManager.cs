using SWP.Domain.Models.Log;
using SWP.Domain.Models.Portal;
using System.Threading.Tasks;

namespace SWP.Domain.Infrastructure.Portal
{
    public interface IPortalManager
    {
        BillingDetail GetBillingDetail(string userId);
        Task<BillingDetail> UpdateBillingDetail(BillingDetail details);
        Task<int> DeleteBillingDetail(string userId);
        Task<int> ClearCustomerData(string userId);
        Task<int> CreateActivity(Activity details);
    }
}
