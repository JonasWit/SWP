using Microsoft.AspNetCore.Identity;
using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Models.Log;
using SWP.Domain.Models.Portal;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.DataBase.Managers
{
    public class PortalManager : DataManagerBase, IPortalManager
    {
        private readonly IAppUserManager _appUserManager;

        public PortalManager(ApplicationDbContext context, IAppUserManager appUserManager) : base(context)
        {
            _appUserManager = appUserManager;
        }

        public Task<int> ClearCustomerData(string userId)
        {
            var details = _context.BillingDetails.FirstOrDefault(x => x.UserId == userId);
            _context.BillingDetails.Remove(details);
            return _context.SaveChangesAsync();
        }

        public Task<Activity> CreateActivity(Activity details)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteBillingDetail(string userId)
        {
            throw new NotImplementedException();
        }

        public BillingDetail GetBillingDetail(string userId) => _context.BillingDetails.FirstOrDefault(x => x.UserId == userId);

        public async Task<BillingDetail> UpdateBillingDetail(BillingDetail details)
        {
            if (_context.BillingDetails.Any(x => x.UserId == details.UserId))
            {
                var record = _context.BillingDetails.FirstOrDefault(x => x.UserId == details.UserId);

                record.Address = details.Address;
                record.AddressCorrespondence = details.AddressCorrespondence;
                record.City = details.City;
                record.CompanyFullName = details.CompanyFullName;
                record.Country = details.Country;
                record.KRS = details.KRS;
                record.Name = details.Name;
                record.NIP = details.NIP;
                record.PhoneNumber = details.PhoneNumber;
                record.PostCode = details.PostCode;
                record.REGON = details.REGON;
                record.Surname = details.Surname;
                record.Vivodership = details.Vivodership;
                record.Updated = DateTime.Now;
                record.UpdatedBy = details.UserId;

                _context.BillingDetails.Update(record);
                await _context.SaveChangesAsync();

                return record;
            }
            else
            {
                details.Created = DateTime.Now;
                details.CreatedBy = details.UserId;
                details.Updated = DateTime.Now;
                details.UpdatedBy = details.UserId;

                _context.BillingDetails.Add(details);
                await _context.SaveChangesAsync();

                return details;
            }
        }
    }
}
