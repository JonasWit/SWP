using SWP.Domain.Models.SWPLegal;
using System;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data
{
    public class ReminderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ParentCaseName { get; set; }
        public string ParentClientName { get; set; }
        public string Message { get; set; }
        public bool Active { get; set; }
        public int Priority { get; set; }

        public int CaseId { get; set; }
        public bool IsDeadline { get; set; }

        public string UpdatedDescription => $"Updated on {Updated} by {UpdatedBy}";
        public string CreatedDescription => $"Created on {Created}";

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }

        public static implicit operator ReminderViewModel(Reminder input) =>
            new ReminderViewModel
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
                Start = input.Start,
                End = input.End,
                IsDeadline = input.IsDeadline,
                ParentCaseName = input.Case?.Name,
                ParentClientName = input.Case?.Client?.Name,
                CreatedBy = input.CreatedBy
            };

        public static implicit operator Reminder(ReminderViewModel input) =>
            new Reminder
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
                Start = input.Start,
                End = input.End,
                IsDeadline = input.IsDeadline,
                CreatedBy = input.CreatedBy
            };
    }
}
