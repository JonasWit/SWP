using SWP.Domain.Infrastructure.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.CashMovements
{
    [TransientService]
    public class DeleteCashMovement : LegalActionsBase
    {
        public DeleteCashMovement(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<int> Delete(int id) => _legalManager.DeleteCashMovement(id);
    }
}
