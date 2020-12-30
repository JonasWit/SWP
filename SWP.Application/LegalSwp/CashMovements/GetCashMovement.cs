using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;

namespace SWP.Application.LegalSwp.CashMovements
{
    [TransientService]
    public class GetCashMovement : LegalActionsBase
    {
        public GetCashMovement(ILegalManager legalManager) : base(legalManager)
        {
        }

        public CashMovement Get(int id) => _legalManager.GetCashMovement(id);
    }
}
