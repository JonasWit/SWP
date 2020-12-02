using Microsoft.AspNetCore.Identity;
using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Models.Log;
using SWP.Domain.Models.Portal;
using System;
using System.Threading.Tasks;

namespace SWP.DataBase.Managers
{
    public class PortalManager : DataManagerBase, IPortalManager
    {
        private readonly UserManager<IdentityUser> _userManager;

        public PortalManager(ApplicationDbContext context, UserManager<IdentityUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public async Task<int> ClearCustomerData(string customerId)
        {
           var user = await _userManager.FindByIdAsync(customerId);

            //todo: handle all data deletion!





            throw new NotImplementedException();
        }

        public Task<Activity> CreateActivity(Activity details)
        {
            throw new NotImplementedException();
        }

        public Task<BillingDetail> CreateBillingDetail(BillingDetail details)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteBillingDetail(int customerId)
        {
            throw new NotImplementedException();
        }

        public BillingDetail GetBillingDetail(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<BillingDetail> UpdateBillingDetail(BillingDetail details)
        {
            throw new NotImplementedException();
        }
    }
}
