using SWP.Domain.Models.Log;
using SWP.Domain.Models.Portal;
using System.Threading.Tasks;

namespace SWP.Domain.Infrastructure.Portal
{
    public interface IPortalManager
    {
        BillingDetail GetBillingDetail(int customerId);
        Task<BillingDetail> CreateBillingDetail(BillingDetail details);
        Task<BillingDetail> UpdateBillingDetail(BillingDetail details);
        Task<int> DeleteBillingDetail(int customerId);
        Task<int> ClearCustomerData(string customerId);
        Task<Activity> CreateActivity(Activity details);
    }
}
