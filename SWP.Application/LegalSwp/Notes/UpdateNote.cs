using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.SWPLegal;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Notes
{
    [TransientService]
    public class UpdateNote
    {
        private readonly ILegalSwpManager legalSwpManager;
        public UpdateNote(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<Note> Update(Request request)
        {
            var note = legalSwpManager.GetNote(request.Id);

            note.Name = request.Name;
            note.Priority = request.Priority;
            note.Updated = DateTime.Now;
            note.UpdatedBy = request.UpdatedBy;
            note.Message = request.Message;
            note.UpdatedBy = request.UpdatedBy;

            return legalSwpManager.UpdateNote(note);
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
