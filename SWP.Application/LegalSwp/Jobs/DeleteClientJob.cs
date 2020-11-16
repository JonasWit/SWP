using SWP.Domain.Infrastructure.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Jobs
{
    [TransientService]
    public class DeleteClientJob
    {
        private readonly ILegalManager legalSwpManager;
        public DeleteClientJob(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<int> Delete(int id) => legalSwpManager.DeleteClientJob(id);



    }
}
