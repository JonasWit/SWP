using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.SWPLegal;
using System.Collections.Generic;

namespace SWP.Application.LegalSwp.CashMovements
{
    [TransientService]
    public class GetCashMovements
    {
        private readonly ILegalManager legalSwpManager;
        public GetCashMovements(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public List<CashMovement> Get(int clientId) => legalSwpManager.GetCashMovementsForClient(clientId);
    }
}
