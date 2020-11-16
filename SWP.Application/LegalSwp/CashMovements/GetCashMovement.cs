using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.SWPLegal;

namespace SWP.Application.LegalSwp.CashMovements
{
    [TransientService]
    public class GetCashMovement
    {
        private readonly ILegalManager legalSwpManager;
        public GetCashMovement(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public CashMovement Get(int id) => legalSwpManager.GetCashMovement(id);
    }
}
