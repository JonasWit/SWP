using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.CashMovements
{
    [TransientService]
    public class DeleteCashMovement
    {
        private readonly ILegalSwpManager legalSwpManager;
        public DeleteCashMovement(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<int> Delete(int id) => legalSwpManager.DeleteCashMovement(id);
    }
}
