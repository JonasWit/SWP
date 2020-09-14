using SWP.Domain.Models.SWPLegal;
using System;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data
{
    public class NoteViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public bool Active { get; set; }
        public int Priority { get; set; }

        public int CaseId { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }

        public static implicit operator NoteViewModel(Note input) =>
            new NoteViewModel
            {
                Id = input.Id,
                Name = input.Name,
                Message = input.Message,
                Active = input.Active,
                Priority = input.Priority,
                CaseId = input.CaseId,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy
            };
    }
}
