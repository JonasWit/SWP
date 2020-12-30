using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.ContactPeopleAdmin
{
    [TransientService]
    public class CreateContactPerson : LegalActionsBase
    {
        public CreateContactPerson(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<ClientContactPerson> CreateContactPersonForClient(int clientId, Request request) =>
            _legalManager.CreateClientContactPerson(clientId, new ClientContactPerson
            {
                Name = request.Name,
                Surname = request.Surname,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                AlternativePhoneNumber = request.AlternativePhoneNumber,
                Email = request.Email,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                UpdatedBy = request.UpdatedBy,
                CreatedBy = request.UpdatedBy
            });

        public Task<CaseContactPerson> CreateContactPersonForCase(int clientId, Request request) =>
            _legalManager.CreateCaseContactPerson(clientId, new CaseContactPerson
            {
                Name = request.Name,
                Surname = request.Surname,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                AlternativePhoneNumber = request.AlternativePhoneNumber,
                Email = request.Email,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                UpdatedBy = request.UpdatedBy,
                CreatedBy = request.UpdatedBy
            });

        public class Request
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
            public string AlternativePhoneNumber { get; set; }
            public string Email { get; set; }
            public string UpdatedBy { get; set; }
        }
    }
}
