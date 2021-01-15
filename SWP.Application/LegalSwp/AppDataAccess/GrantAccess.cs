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

        public Task<int> GrantAccessToCase(string userId, int caseId) => _legalManager.GrantAccessToCase(userId, caseId);
        public Task<int> GrantAccessToClient(string userId, int clientId) => _legalManager.GrantAccessToClient(userId, clientId);

        public Task<int> GrantAccessToCases(string userId, List<int> caseIds) => caseIds.Count == 0 ? Task.FromResult(0) : _legalManager.GrantAccessToCases(userId, caseIds);
        public Task<int> GrantAccessToClients(string userId, List<int> clientIds) => clientIds.Count == 0 ? Task.FromResult(0) : _legalManager.GrantAccessToClients(userId, clientIds);

    }
}
