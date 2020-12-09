using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Models.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.DataBase.Managers
{
    public class PortalManager : DataManagerBase, IPortalManager
    {
        private readonly IAppUserManager _appUserManager;

        public PortalManager(AppContext context, IAppUserManager appUserManager) : base(context)
        {
            _appUserManager = appUserManager;
        }

        public Task<int> DeleteBillingDetail(string userId)
        {
            var entity = _context.BillingDetails.FirstOrDefault(x => x.UserId == userId);
            _context.BillingDetails.Remove(entity);
            return _context.SaveChangesAsync();
        }

        public BillingDetail GetBillingDetail(string userId) => _context.BillingDetails.FirstOrDefault(x => x.UserId == userId);

        public Task<int> DeleteLicense(int id)
        {
            var entity = _context.UserLicenses.FirstOrDefault(x => x.Id == id);
            _context.UserLicenses.Remove(entity);
            return _context.SaveChangesAsync();
        }

        public List<UserLicense> GetLicenses(string userId) => _context.UserLicenses.Where(x => x.UserId == userId).ToList();

        public async Task<BillingDetail> UpdateBillingDetail(BillingDetail updatedEntity)
        {
            if (_context.BillingDetails.Any(x => x.UserId == updatedEntity.UserId))
            {
                var record = _context.BillingDetails.FirstOrDefault(x => x.UserId == updatedEntity.UserId);

                record.Address = updatedEntity.Address;
                record.AddressCorrespondence = updatedEntity.AddressCorrespondence;
                record.City = updatedEntity.City;
                record.CompanyFullName = updatedEntity.CompanyFullName;
                record.Country = updatedEntity.Country;
                record.KRS = updatedEntity.KRS;
                record.Name = updatedEntity.Name;
                record.NIP = updatedEntity.NIP;
                record.PhoneNumber = updatedEntity.PhoneNumber;
                record.PostCode = updatedEntity.PostCode;
                record.REGON = updatedEntity.REGON;
                record.Surname = updatedEntity.Surname;
                record.Vivodership = updatedEntity.Vivodership;
                record.Updated = DateTime.Now;
                record.UpdatedBy = updatedEntity.UserId;

                _context.BillingDetails.Update(record);
                await _context.SaveChangesAsync();

                return record;
            }
            else
            {
                updatedEntity.Created = DateTime.Now;
                updatedEntity.CreatedBy = updatedEntity.UserId;
                updatedEntity.Updated = DateTime.Now;
                updatedEntity.UpdatedBy = updatedEntity.UserId;

                _context.BillingDetails.Add(updatedEntity);
                await _context.SaveChangesAsync();

                return updatedEntity;
            }
        }

        public async Task<UserLicense> UpdateLicense(UserLicense updatedEntity)
        {
            if (_context.UserLicenses.Any(x => x.Id == updatedEntity.Id))
            {
                var record = _context.UserLicenses.FirstOrDefault(x => x.Id == updatedEntity.Id);

                record.ValidTo = updatedEntity.ValidTo;
                record.Type = updatedEntity.Type;
                record.Application = updatedEntity.Application;
                record.RelatedUsers = updatedEntity.RelatedUsers;

                record.Updated = DateTime.Now;
                record.UpdatedBy = updatedEntity.UpdatedBy;

                _context.UserLicenses.Update(record);
                await _context.SaveChangesAsync();

                return record;
            }
            else
            {
                return null;
            }
        }

        public async Task<UserLicense> CreateLicense(UserLicense license)
        {
            _context.UserLicenses.Add(license);
            await _context.SaveChangesAsync();
            return license;
        }

        public Task<int> DeleteLicense(string userId)
        {
            var entity = _context.UserLicenses.FirstOrDefault(x => x.UserId == userId);
            _context.UserLicenses.Remove(entity);
            return _context.SaveChangesAsync();
        }
    }
}
