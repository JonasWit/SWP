using SWP.Domain.Models.LegalApp;
using System;

namespace SWP.UI.Components.ViewModels.LegalApp
{
    public class NoteViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public bool Active { get; set; }
        public int Priority { get; set; }

        public int CaseId { get; set; }

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
