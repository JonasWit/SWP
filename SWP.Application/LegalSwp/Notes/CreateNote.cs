﻿using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Notes
{
    [TransientService]
    public class CreateNote : LegalActionsBase
    {
        public CreateNote(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<Note> Create(int caseId, Request request) =>
            _legalManager.CreateNote(caseId, new Note
            {
                Name = request.Name,
                Message = request.Message,
                Priority = request.Priority,
                Active = true,
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

            public DateTime Created { get; set; }

            public DateTime Updated { get; set; }

            [MaxLength(50)]
            [Required]
            public string UpdatedBy { get; set; }
        }
    }
}
