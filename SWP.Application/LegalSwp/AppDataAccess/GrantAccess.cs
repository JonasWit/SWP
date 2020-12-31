using SWP.Domain.Infrastructure.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.AppDataAccess
{
    [TransientService]
    public class GrantAccess : LegalActionsBase
    {
        public GrantAccess(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<int> GrantAccessToCase(string userId, int caseId) => GrantAccessToCase(userId, caseId);
        public Task<int> GrantAccessToClient(string userId, int clientId) => GrantAccessToClient(userId, clientId);
        public Task<int> GrantAccessToPanel(string userId, int panelId) => GrantAccessToPanel(userId, panelId);

        public Task<int> GrantAccessToCases(string userId, List<int> caseIds) => GrantAccessToCases(userId, caseIds);
        public Task<int> GrantAccessToClients(string userId, List<int> clientIds) => GrantAccessToClients(userId, clientIds);
        public Task<int> GrantAccessToRanels(string userId, List<int> panelsIds) => GrantAccessToRanels(userId, panelsIds);

    }
}
