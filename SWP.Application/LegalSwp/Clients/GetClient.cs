using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;

namespace SWP.Application.LegalSwp.Clients
{
    [TransientService]
    public class GetClient : LegalActionsBase
    {
        public GetClient(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Client Get(int id) => _legalManager.GetClient(id);
        public Client GetCleanClient(int id) => _legalManager.GetCleanClient(id);
        public string GetClientName(int id) => _legalManager.GetClientName(id);
    }
}
