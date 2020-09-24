using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.LegalSwp.CashMovements
{
    [TransientService]
    public class GetCashMovements
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetCashMovements(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public List<CashMovement> Get(int clientId) => legalSwpManager.GetCashMovementsForClient(clientId);







    }
}
