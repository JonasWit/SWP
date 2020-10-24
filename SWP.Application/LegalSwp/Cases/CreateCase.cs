using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Cases
{
    [TransientService]
    public class CreateCase
    {
        private readonly ILegalSwpManager legalSwpManager;
        public CreateCase(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<Case> Create(int ClientId, string profile, Request request) => 
            legalSwpManager.CreateCase(ClientId, profile, new Case
            { 
                Name = request.Name,
                Signature = string.IsNullOrEmpty(request.Signature) ? "Brak" : request.Signature,
                CaseType = string.IsNullOrEmpty(request.CaseType) ? "Standard" : request.CaseType,
                Description = string.IsNullOrEmpty(request.Description) ? "Brak" : request.Description,
                Active = true,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                UpdatedBy = request.UpdatedBy,
                CreatedBy = request.UpdatedBy
            });

        public class Request
        {
            public string Name { get; set; }

            public string Signature { get; set; }

            public string CaseType { get; set; }

            public string Description { get; set; }

            public string UpdatedBy { get; set; }
        }


    }
}
