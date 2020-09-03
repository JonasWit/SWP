using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Reminders
{
    [TransientService]
    public class CreateReminder
    {
        private readonly ILegalSwpManager legalSwpManager;
        public CreateReminder(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<Reminder> Create(int caseId, Request request) =>
            legalSwpManager.CreateReminder(caseId, new Reminder
            {
                Name = request.Name,
                Start = request.Start,
                End = request.End,
                IsDeadline = request.IsDeadline,
                Message = request.Message,
                Priority = request.Priority,
                Active = true,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                UpdatedBy = request.UpdatedBy
            });

        public class Request
        {
            [MaxLength(50)]
            [Required]
            public string Name { get; set; }
            [MaxLength(500)]
            public string Message { get; set; }
            public bool Active { get; set; }
            public int Priority { get; set; }
            public bool IsDeadline { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public DateTime Created { get; set; }
            public DateTime Updated { get; set; }
            [MaxLength(50)]
            [Required]
            public string UpdatedBy { get; set; }
        }

    }
}
