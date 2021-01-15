using SWP.Domain.Models.LegalApp;
using System;

namespace SWP.UI.Components.ViewModels.LegalApp
{
    public class ReminderViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string ParentCaseName { get; set; } = "";
        public string ParentClientName { get; set; } = "";
        public string Message { get; set; }
        public bool Active { get; set; }
        public int Priority { get; set; }
        public int CaseId { get; set; }
        public bool IsDeadline { get; set; }
        public string Duration => (End - Start).ToString(@"hh\:mm") == @"00:00" ? "" : (End - Start).ToString(@"hh\:mm");
        public string DisplayText => $@"{(ParentClientName.Length > 12 ? $@"{ParentClientName.Substring(0, 12)}..." : ParentClientName)} - {(Name.Length > 12 ? $@"{Name.Substring(0, 12)}..." : Name)}";
        public string DisplayTextShort => $@"{(Name.Length > 12 ? $@"{Name.Substring(0, 12)}..." : Name)}";
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public static implicit operator ReminderViewModel(Reminder input) =>
            new ReminderViewModel
            {
                Id = input.Id,
                Name = input.Name,
                Message = input.Message,
                Priority = input.Priority,
                CaseId = input.CaseId,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                Start = input.Start,
                End = input.End,
                IsDeadline = input.IsDeadline,
                ParentCaseName = input.Case?.Name,
                ParentClientName = input.Case?.Client?.Name,
                CreatedBy = input.CreatedBy,
                Active = true
            };

        public static implicit operator Reminder(ReminderViewModel input) =>
            new Reminder
            {
                Id = input.Id,
                Name = input.Name,
                Message = input.Message,
                Priority = input.Priority,
                CaseId = input.CaseId,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                Start = input.Start,
                End = input.End,
                IsDeadline = input.IsDeadline,
                CreatedBy = input.CreatedBy
            };
    }
}
