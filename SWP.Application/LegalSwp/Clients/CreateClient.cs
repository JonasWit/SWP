using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Clients
{
    [TransientService]
    public class CreateClient
    {
        private readonly ILegalSwpManager legalSwpManager;
        public CreateClient(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<Client> Do(Request request) => legalSwpManager.CreateClient(new Client
        {
            Name = request.Name,
            ProfileClaim = request.ProfileClaim,
            Address = string.IsNullOrEmpty(request.Address) ? "Brak" : request.Address,
            Email = string.IsNullOrEmpty(request.Email) ? "Brak" : request.Email,
            PhoneNumber = string.IsNullOrEmpty(request.PhoneNumber) ? "Brak" : request.PhoneNumber,
            Cases = new List<Case>(),
            Jobs = new List<ClientJob>(),
            Created = DateTime.Now,
            Updated = DateTime.Now,
            UpdatedBy = request.UpdatedBy,
            CreatedBy = request.UpdatedBy
        });

        public class Request
        {
            [Required(ErrorMessage = "Nazwa Klienta nie może być pusta!")]
            public string Name { get; set; }
            public string ProfileClaim { get; set; }

            public string Address { get; set; }

            [DataType(DataType.EmailAddress)]
            [EmailAddress(ErrorMessage = "Niepoprawny adres Email!")]
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string UpdatedBy { get; set; }
        }
    }
}
