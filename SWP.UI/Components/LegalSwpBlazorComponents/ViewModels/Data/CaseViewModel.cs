using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data
{
    public class CaseViewModel
    {
        public int Id { get; set; }
        public string IdStr => Id.ToString();
        public string Name { get; set; }
        public string Signature { get; set; }
        public string CaseType { get; set; }
        public string Description { get; set; }

        public bool Active { get; set; }

        public List<ReminderViewModel> Reminders { get; set; }
        public List<NoteViewModel> Notes { get; set; }
        public NoteViewModel SelectedNote { get; set; }

        public int CustomerId { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }


        public static implicit operator CaseViewModel(Case input) =>
            new CaseViewModel
            {
                Id = input.Id,
                Name = input.Name,
                Signature = input.Signature,
                CaseType = input.CaseType,
                Description = input.Description,
                Active = input.Active,
                Reminders = input.Reminders == null ? new List<ReminderViewModel>() : input.Reminders.Select(x => (ReminderViewModel)x).ToList(),
                Notes = input.Notes == null ? new List<NoteViewModel>() : input.Notes.Select(x => (NoteViewModel)x).ToList(),
                CustomerId = input.CustomerId,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy
            };
    }
}
