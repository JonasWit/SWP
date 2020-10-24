using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.SWPLegal;

namespace SWP.Application.LegalSwp.Clients
{
    [TransientService]
    public class GetClient
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetClient(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Client Get(int id) => legalSwpManager.GetClient(id);
        public string GetClientName(int id) => legalSwpManager.GetClientName(id);
    }
}
