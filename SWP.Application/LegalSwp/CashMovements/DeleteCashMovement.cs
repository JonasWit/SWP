using SWP.Domain.Infrastructure.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.CashMovements
{
    [TransientService]
    public class DeleteCashMovement
    {
        private readonly ILegalManager legalSwpManager;
        public DeleteCashMovement(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<int> Delete(int id) => legalSwpManager.DeleteCashMovement(id);
    }
}
