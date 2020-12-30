using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Notes
{
    [TransientService]
    public class ArchiveNote : LegalActionsBase
    {
        public ArchiveNote(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<int> ArchivizeNore(int noteId, string user) => _legalManager.ArchivizeNote(noteId, user);
        public Note RecoverClientJob(int noteId, string user) => _legalManager.RecoverNote(noteId, user);
    }
}
