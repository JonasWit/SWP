using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data
{
    public class CaseViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Signature { get; set; }
        public string CaseType { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public List<ReminderViewModel> Reminders { get; set; }
        public List<NoteViewModel> Notes { get; set; }
        public List<NoteViewModel> ArchivedNotes { get; set; }
        public List<ContactPersonViewModel> ContactPeople { get; set; }
        public long ClientId { get; set; }

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
                Notes = input.Notes == null ? new List<NoteViewModel>() : input.Notes.Where(x => x.Active).Select(x => (NoteViewModel)x).ToList(),
                ArchivedNotes = input.Notes == null ? new List<NoteViewModel>() : input.Notes.Where(x => !x.Active).Select(x => (NoteViewModel)x).ToList(),
                ContactPeople = input.ContactPeople == null ? new List<ContactPersonViewModel>() : input.ContactPeople.Select(x => (ContactPersonViewModel)x).ToList(),
                ClientId = input.ClientId,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy
            };
    }
}
