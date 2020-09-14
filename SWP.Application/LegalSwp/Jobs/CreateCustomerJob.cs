﻿using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Jobs
{
    [TransientService]
    public class CreateCustomerJob
    {
        private readonly ILegalSwpManager legalSwpManager;
        public CreateCustomerJob(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<CustomerJob> Create(Request request) => 
            legalSwpManager.CreateCustomerJob(request.CustomerId, request.ProfileClaim, new CustomerJob
            {
                Active = true,
                Priority = request.Priority,
                Description = request.Description,
                Name = request.Name,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                UpdatedBy = request.UpdatedBy,
                CreatedBy = request.UpdatedBy
            });

        public class Request
        {
            [Required(ErrorMessage = "Nazwa nie może być pusta!")]
            public string Name { get; set; }
            public string Description { get; set; }
            public bool Active { get; set; }
            public int Priority { get; set; }

            public int CustomerId { get; set; }
            public string ProfileClaim { get; set; }

            public DateTime Created { get; set; }
            public DateTime Updated { get; set; }
            public string UpdatedBy { get; set; }
        }
    }


}
