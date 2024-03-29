﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SWP.Domain.Enums;
using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Models.Portal;
using SWP.Domain.Models.Portal.Communication;
using SWP.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.DataBase.Managers
{
    public class PortalManager : DataManagerBase, IPortalManager
    {
        private readonly UserManager<IdentityUser> _userManager;

        public PortalManager(AppContext context, UserManager<IdentityUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public Task<int> DeleteBillingDetail(string userId)
        {
            var entity = _context.BillingDetails.FirstOrDefault(x => x.UserId == userId);

            if (entity is not null)
            {
                _context.BillingDetails.Remove(entity);
                return _context.SaveChangesAsync();
            }
            else
            {
                return Task.FromResult(0);    
            }
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

        public Task<int> DeleteLicenses(string userId)
        {
            var entities = _context.UserLicenses.Where(x => x.UserId == userId).ToList();

            if (entities.Count != 0)
            {
                _context.UserLicenses.RemoveRange(entities);
                return _context.SaveChangesAsync();
            }
            else
            {
                return Task.FromResult(0);
            }
        }

        public bool ClaimExists(string claimType, string claimValue)
        {
            if (_context.UserClaims.Any(x => x.ClaimType == claimType && x.ClaimValue == claimValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<string> GetAllProfiles() =>
            _context.UserClaims.Where(x => x.ClaimType == ClaimType.Profile.ToString()).Select(x => x.ClaimValue).Distinct().ToList();

        public List<UserLicense> GetLicensesForProfile(string profile)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetParentAccountId(IdentityUser relatedUser, Claim profileClaim)
        {
            if (relatedUser == null || profileClaim == null)
            {
                return null;
            }

            var profileUsers = await _userManager.GetUsersForClaimAsync(profileClaim) as List<IdentityUser>;

            foreach (var profileUser in profileUsers)
            {
                var claims = await _userManager.GetClaimsAsync(profileUser) as List<Claim>;
                if (claims.Any(x => x.Type == ClaimType.Status.ToString() && x.Value == UserStatus.RootClient.ToString()))
                {
                    return profileUser.Id;
                }
            }

            return null;
        }

        public List<string> GetRelatedUsers(string profile)
        {
            throw new NotImplementedException();
        }

        public Task<IList<IdentityUser>> GetUsersForProfile(Claim claim) => _userManager.GetUsersForClaimAsync(claim);

        public Task<IdentityUser> GetUserByID(string id) => _userManager.FindByIdAsync(id);

        public Task<IdentityUser> GetUserByName(string name) => _userManager.FindByNameAsync(name);

        public async Task<string> GetUserProfileByID(string userId)
        {
            var claims = await _userManager.GetClaimsAsync(await GetUserByID(userId)) as List<Claim>;
            var profileClaim = claims.FirstOrDefault(x => x.Type == ClaimType.Profile.ToString());
            return profileClaim.Value;
        }

        public async Task<ManagerActionResult> ChangeProfileName(Claim oldProfile, string newProfile)
        {
            //not used for now
            try
            {
                var users = await GetUsersForProfile(oldProfile) as List<IdentityUser>;

                foreach (var user in users)
                {
                    var removeResult = await _userManager.RemoveClaimAsync(user, oldProfile);

                    if (removeResult.Succeeded)
                    {
                        var newClaim = new Claim(ClaimType.Profile.ToString(), newProfile.Trim());
                        var addResult = await _userManager.AddClaimAsync(user, newClaim);

                        if (!addResult.Succeeded)
                        {
                            throw new Exception($"Error when adding new Profile for user {user.UserName}, Issues: {string.Join("; ", addResult.Errors.Select(x => x.Description).ToList())}");
                        }
                    }
                    else
                    {
                        throw new Exception($"Error when removing old Profile for user {user.UserName}, Issues: { string.Join("; ", removeResult.Errors.Select(x => x.Description).ToList()) }");
                    }
                }

                var clientsToUpdate = _context.Clients.Where(x => x.ProfileClaim == oldProfile.Value).ToList();

                foreach (var clinet in clientsToUpdate)
                {
                    clinet.ProfileClaim = newProfile;
                }

                _context.UpdateRange(clientsToUpdate);
                await _context.SaveChangesAsync();
                return null;
            }
            catch (ManagerActionResult ex)
            {
                return ex;
            }
        }

        #region Requests Management

        public ClientRequest GetRequestWithMessages(int requestId) =>
            _context.ClientRequests.Include(x => x.Messages).FirstOrDefault(x => x.Id == requestId);
 
        public List<ClientRequest> GetRequestsForClient(string userId) => 
            _context.ClientRequests.Where(x => x.CreatedBy == userId).ToList();

        public Task<int> CreateRequest(ClientRequest clientRequest)
        {
            _context.ClientRequests.Add(clientRequest);
            return _context.SaveChangesAsync();
        }

        public Task<int> CreateRequestMessage(ClientRequestMessage message, int reuqestId)
        {
            var parent = _context.ClientRequests.Include(x => x.Messages).FirstOrDefault(x => x.Id == reuqestId);
            parent.Messages.Add(message);
            return _context.SaveChangesAsync();
        }

        public Task<int> DeleteRequest(int id)
        {
            var entity = _context.ClientRequests.FirstOrDefault(x => x.Id == id);
            _context.ClientRequests.Remove(entity);
            return _context.SaveChangesAsync();
        }

        public Task<int> DeleteRequest(string userId)
        {
            _context.ClientRequests.RemoveRange(_context.ClientRequests.Where(x => x.CreatedBy == userId));
            return _context.SaveChangesAsync();
        }

        public List<TResult> GetRequests<TResult>(Func<ClientRequest, TResult> selector, Func<ClientRequest, bool> predicate) => 
            _context.ClientRequests.Where(predicate).Select(selector).ToList();

        public List<ClientRequest> GetRequests() => _context.ClientRequests.ToList();

        public Task<int> UpdateRequest(ClientRequest request)
        {
            var entity = _context.ClientRequests.FirstOrDefault(x => x.Id.Equals(request.Id));

            entity.UpdatedBy = request.UpdatedBy;
            entity.Updated = request.Updated;
            entity.Status = request.Status;

            _context.ClientRequests.Update(entity);
            return _context.SaveChangesAsync();
        }

        #endregion
    }
}
