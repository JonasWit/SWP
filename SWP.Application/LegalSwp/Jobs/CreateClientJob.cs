using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Jobs
{
    [TransientService]
    public class CreateClientJob : LegalActionsBase
    {
        public CreateClientJob(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<ClientJob> Create(Request request) => 
            _legalManager.CreateClientJob(request.ClientId, new ClientJob
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
            public int Priority { get; set; }
            public int ClientId { get; set; }
            public DateTime Created { get; set; }
            public DateTime Updated { get; set; }
            public string UpdatedBy { get; set; }
        }
    }


}
