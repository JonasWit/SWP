using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.SWPLegal;
using System.Collections.Generic;

namespace SWP.Application.LegalSwp.Clients
{
    [TransientService]
    public class GetClients
    {
        private readonly ILegalManager legalSwpManager;
        public GetClients(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public List<Client> GetClientsWithoutData(string claim, bool active = true) => legalSwpManager.GetClientsWithoutCases(claim, active);
        public IEnumerable<int> GetClientCasesIds(int id) => legalSwpManager.GetClientCasesIds(id);
        public int CountJobsPerClient(int id) => legalSwpManager.CountJobsPerClient(id);
        public int CountCasesPerClient(int clientId) => legalSwpManager.CountCasesPerClient(clientId);
    }
}
