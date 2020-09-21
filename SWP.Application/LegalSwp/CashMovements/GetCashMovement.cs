using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.LegalSwp.CashMovements
{
    [TransientService]
    public class GetCashMovement
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetCashMovement(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public CashMovement Get(int id) => legalSwpManager.GetCashMovement(id);
    }
}
