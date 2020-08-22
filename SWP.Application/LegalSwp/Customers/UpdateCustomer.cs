using SWP.Domain.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Customers
{
    [TransientService]
    public class UpdateCustomer
    {
        private readonly ILegalSwpManager legalSwpManager;
        public UpdateCustomer(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<int> Update(Request request)
        {
            var record = legalSwpManager.GetCustomer(request.Id, request.ProfileClaim, x => x);

            record.Address = request.Address;
            record.Active = request.Active;
            record.Email = request.Email;
            record.Name = request.Name;
            record.PhoneNumber = request.PhoneNumber;
            record.Updated = System.DateTime.Now;
            record.UpdatedBy = request.UpdatedBy;

            return legalSwpManager.UpdateCustomer(record);
        }          

        public class Request
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Nazwa Klienta nie może być pusta!")]
            public string Name { get; set; }
            public bool Active { get; set; }

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
