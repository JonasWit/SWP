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

        public Task<int> RevokeAccessToCase(string userId, int caseId) => RevokeAccessToCase(userId, caseId);
        public Task<int> RevokeAccessToClient(string userId, int clientId) => RevokeAccessToClient(userId, clientId);
        public Task<int> RevokeAccessToPanel(string userId, int panelId) => RevokeAccessToPanel(userId, panelId);

        public Task<int> RevokeAccessToCases(string userId, List<int> caseIds) => RevokeAccessToCases(userId, caseIds);
        public Task<int> RevokeAccessToClients(string userId, List<int> clientIds) => RevokeAccessToClients(userId, clientIds);
        public Task<int> RevokeAccessToRanels(string userId, List<int> panelsIds) => RevokeAccessToRanels(userId, panelsIds);

    }
}
