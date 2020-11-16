using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Cases
{
    [TransientService]
    public class UpdateCase
    {
        private readonly ILegalManager legalSwpManager;
        public UpdateCase(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<Case> Update(Request request)
        {
            var c = legalSwpManager.GetCase(request.Id);

            c.Name = request.Name;
            c.Description = request.Description;
            c.Signature = request.Signature;
            c.CaseType = request.CaseType;
            c.Updated = DateTime.Now;
            c.UpdatedBy = request.UpdatedBy;

            return legalSwpManager.UpdateCase(c);
        }

        public class Request
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Signature { get; set; }
            public string CaseType { get; set; }
            public string Description { get; set; }
            public string UpdatedBy { get; set; }
        }


    }
}
