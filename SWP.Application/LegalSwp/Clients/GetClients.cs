using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Clients
{
    [TransientService]
    public class GetClients
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetClients(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public List<Client> GetClientsWithData(string claim) => legalSwpManager.GetClients(claim, x => x);
        public List<Client> GetClientsWithoutData(string claim) => legalSwpManager.GetClientsWithoutCases(claim);
        public IEnumerable<int> GetClientCasesIds(int id) => legalSwpManager.GetClientCasesIds(id);
        public int? CountJobsPerClient(int id) => legalSwpManager.CountJobsPerClient(id);
    }
}
