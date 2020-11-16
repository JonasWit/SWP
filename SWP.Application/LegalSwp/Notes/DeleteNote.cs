using SWP.Domain.Infrastructure.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Notes
{
    [TransientService]
    public class DeleteNote
    {
        private readonly ILegalManager legalSwpManager;
        public DeleteNote(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<int> Delete(int id) => legalSwpManager.DeleteNote(id);
    }
}
