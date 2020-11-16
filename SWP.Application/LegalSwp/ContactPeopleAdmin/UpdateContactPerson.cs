using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.ContactPeopleAdmin
{
    [TransientService]
    public class UpdateContactPerson
    {
        private readonly ILegalManager legalSwpManager;
        public UpdateContactPerson(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<CaseContactPerson> UpdateForCase(Request request)
        {
            var c = legalSwpManager.GetCaseContactPerson(request.Id);

            c.Name = request.Name;
            c.Surname = request.Surname;
            c.Address = request.Address;
            c.PhoneNumber = request.PhoneNumber;
            c.AlternativePhoneNumber = request.AlternativePhoneNumber;
            c.Email = request.Email;
            c.Updated = request.Updated;
            c.UpdatedBy = request.UpdatedBy;

            return legalSwpManager.UpdateCaseContactPerson(c);
        }

        public Task<ClientContactPerson> UpdateForClient(Request request)
        {
            var c = legalSwpManager.GetClientContactPerson(request.Id);

            c.Name = request.Name;
            c.Surname = request.Surname;
            c.Address = request.Address;
            c.PhoneNumber = request.PhoneNumber;
            c.AlternativePhoneNumber = request.AlternativePhoneNumber;
            c.Email = request.Email;
            c.Updated = request.Updated;
            c.UpdatedBy = request.UpdatedBy;

            return legalSwpManager.UpdateClientContactPerson(c);
        }

        public class Request
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
            public string AlternativePhoneNumber { get; set; }
            public string Email { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime Updated { get; set; }
        }
    }
}
