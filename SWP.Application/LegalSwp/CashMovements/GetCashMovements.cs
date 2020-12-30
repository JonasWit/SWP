using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System.Collections.Generic;

namespace SWP.Application.LegalSwp.CashMovements
{
    [TransientService]
    public class GetCashMovements : LegalActionsBase
    {
        public GetCashMovements(ILegalManager legalManager) : base(legalManager)
        {
        }

        public List<CashMovement> Get(int clientId) => _legalManager.GetCashMovementsForClient(clientId);
    }
}
