using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Infrastructure.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.PortalCustomers
{
    [TransientService]
    public class DeleteAccountRelatedData : PortalManagerBase
    {
        private readonly ILegalManager _legalManager;

        public DeleteAccountRelatedData(IPortalManager portalManager, ILegalManager legalManager) : base(portalManager)
        {
            _legalManager = legalManager;
        }

        public async Task<bool> Delete(string userId)
        {
            try
            {
                await _portalManager.DeleteBillingDetail(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception when deleting Billing data: {ex.Message}", ex);
            }

            try
            {
                await _portalManager.DeleteRequest(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception when deleting Requests data: {ex.Message}", ex);
            }

            try
            {
                await _legalManager.DeleteAllAccess(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception when deleting Access data: {ex.Message}", ex);
            }

            try
            {
                await _portalManager.DeleteLicenses(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception when deleting Access data: {ex.Message}", ex);
            }

            return true;
        }
    }
}
