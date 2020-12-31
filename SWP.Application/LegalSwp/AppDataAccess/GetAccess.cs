using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp.AccessControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.AppDataAccess
{
    [TransientService]
    public class GetAccess : LegalActionsBase
    {
        public GetAccess(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<List<AccessToCase>> GetAccessToCase(string userId) => GetAccessToCase( userId);
        public Task<List<AccessToClient>> GetAccessToClient(string userId) => GetAccessToClient(userId);
        public Task<List<AccessToPanel>> GetAccessToPanel(string userId) => GetAccessToPanel(userId);
    }
}
