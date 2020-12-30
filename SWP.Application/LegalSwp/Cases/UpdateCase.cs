using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Cases
{
    [TransientService]
    public class UpdateCase : LegalActionsBase
    {
        public UpdateCase(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<Case> Update(Request request)
        {
            var c = _legalManager.GetCase(request.Id);

            c.Name = request.Name;
            c.Description = request.Description;
            c.Signature = request.Signature;
            c.CaseType = request.CaseType;
            c.Updated = DateTime.Now;
            c.UpdatedBy = request.UpdatedBy;

            return _legalManager.UpdateCase(c);
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
