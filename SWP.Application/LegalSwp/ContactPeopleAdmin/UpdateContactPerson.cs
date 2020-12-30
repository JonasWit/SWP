using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.ContactPeopleAdmin
{
    [TransientService]
    public class UpdateContactPerson : LegalActionsBase
    {
        public UpdateContactPerson(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<CaseContactPerson> UpdateForCase(Request request)
        {
            var c = _legalManager.GetCaseContactPerson(request.Id);

            c.Name = request.Name;
            c.Surname = request.Surname;
            c.Address = request.Address;
            c.PhoneNumber = request.PhoneNumber;
            c.AlternativePhoneNumber = request.AlternativePhoneNumber;
            c.Email = request.Email;
            c.Updated = request.Updated;
            c.UpdatedBy = request.UpdatedBy;

            return _legalManager.UpdateCaseContactPerson(c);
        }

        public Task<ClientContactPerson> UpdateForClient(Request request)
        {
            var c = _legalManager.GetClientContactPerson(request.Id);

            c.Name = request.Name;
            c.Surname = request.Surname;
            c.Address = request.Address;
            c.PhoneNumber = request.PhoneNumber;
            c.AlternativePhoneNumber = request.AlternativePhoneNumber;
            c.Email = request.Email;
            c.Updated = request.Updated;
            c.UpdatedBy = request.UpdatedBy;

            return _legalManager.UpdateClientContactPerson(c);
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
