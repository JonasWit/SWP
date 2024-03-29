﻿using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Reminders
{
    [TransientService]
    public class CreateReminder : LegalActionsBase
    {
        public CreateReminder(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<Reminder> Create(int caseId, Request request) =>
            _legalManager.CreateReminder(caseId, new Reminder
            {
                Name = request.Name,
                Start = request.Start,
                End = request.End,
                IsDeadline = request.IsDeadline,
                Message = request.Message,
                Priority = request.Priority,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                UpdatedBy = request.UpdatedBy,
                CreatedBy = request.UpdatedBy
            });

        public class Request
        {
            [MaxLength(50)]
            [Required]
            public string Name { get; set; }
            [MaxLength(500)]
            public string Message { get; set; }
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
