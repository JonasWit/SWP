using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Notes
{
    [TransientService]
    public class UpdateNote : LegalActionsBase
    {
        public UpdateNote(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<Note> Update(Request request)
        {
            var note = _legalManager.GetNote(request.Id);

            note.Name = request.Name;
            note.Priority = request.Priority;
            note.Updated = DateTime.Now;
            note.UpdatedBy = request.UpdatedBy;
            note.Message = request.Message;
            note.UpdatedBy = request.UpdatedBy;

            return _legalManager.UpdateNote(note);
        }

        public class Request
        {
            public int Id { get; set; }
            [MaxLength(50)]
            [Required]
            public string Name { get; set; }
            [MaxLength(500)]
            public string Message { get; set; }
            public int Priority { get; set; }
            [MaxLength(50)]
            [Required]
            public string UpdatedBy { get; set; }
        }


    }
}
