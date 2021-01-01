using SWP.Domain.Infrastructure.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.AppDataAccess
{
    [TransientService]
    public class RevokeAccess : LegalActionsBase
    {
        public RevokeAccess(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<int> RevokeAllAccesses(string userId) => _legalManager.DeleteAllAccess(userId);

        public Task<int> RevokeAccessToCase(string userId, int caseId) => _legalManager.RevokeAccessToCase(userId, caseId);
        public Task<int> RevokeAccessToClient(string userId, int clientId) => _legalManager.RevokeAccessToClient(userId, clientId);
        public Task<int> RevokeAccessToPanel(string userId, int panelId) => _legalManager.RevokeAccessToPanel(userId, panelId);

        public Task<int> RevokeAccessToCases(string userId, List<int> caseIds) => caseIds.Count == 0 ? Task.FromResult(0) : _legalManager.RevokeAccessToCases(userId, caseIds);
        public Task<int> RevokeAccessToClients(string userId, List<int> clientIds) => clientIds.Count == 0 ? Task.FromResult(0) : _legalManager.RevokeAccessToClients(userId, clientIds);
        public Task<int> RevokeAccessToRanels(string userId, List<int> panelsIds) => panelsIds.Count == 0 ? Task.FromResult(0) : _legalManager.RevokeAccessToRanels(userId, panelsIds);

        public Task<int> DeleteCaseAccessRecords(List<int> caseIds) => caseIds.Count == 0 ? Task.FromResult(0) : _legalManager.DeleteCaseAccessRecords(caseIds);
        public Task<int> DeleteClientAccessRecordsList(List<int> clientIds) => clientIds.Count == 0 ? Task.FromResult(0) : _legalManager.DeleteClientAccessRecords(clientIds);
    }
}
