using SWP.Domain.Infrastructure.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Jobs
{
    [TransientService]
    public class DeleteClientJob : LegalActionsBase
    {
        public DeleteClientJob(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<int> Delete(int id) => _legalManager.DeleteClientJob(id);
    }
}
