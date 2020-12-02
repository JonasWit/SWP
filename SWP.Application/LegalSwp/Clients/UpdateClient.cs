using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Clients
{
    [TransientService]
    public class UpdateClient
    {
        private readonly ILegalManager legalSwpManager;
        public UpdateClient(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<Client> Update(Request request)
        {
            var record = legalSwpManager.GetClient(request.Id);

            record.Address = request.Address;
            record.Active = request.Active;
            record.Email = request.Email;
            record.Name = request.Name;
            record.PhoneNumber = request.PhoneNumber;
            record.Updated = System.DateTime.Now;
            record.UpdatedBy = request.UpdatedBy;

            return legalSwpManager.UpdateClient(record);
        }          

        public class Request
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Nazwa Klienta nie może być pusta!")]
            public string Name { get; set; }

            public bool Active { get; set; }

            public string Address { get; set; }

            [DataType(DataType.EmailAddress)]
            [EmailAddress(ErrorMessage = "Niepoprawny adres Email!")]
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string UpdatedBy { get; set; }
        }
    }
}
