using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.ContactPeopleAdmin
{
    [TransientService]
    public class CreateContactPerson
    {
        private readonly ILegalSwpManager legalSwpManager;
        public CreateContactPerson(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<ContactPerson> CreateContactPersonForClient(int clientId, Request request) =>
            legalSwpManager.CreateClientContactPerson(clientId, new ContactPerson
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
