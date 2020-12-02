using SWP.Domain.Models.LegalApp;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data
{
    public class ContactPersonViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternativePhoneNumber { get; set; }
        public string Email { get; set; }
        public int CaseId { get; set; }
        public int ClientId { get; set; }

        public static implicit operator ContactPersonViewModel(CaseContactPerson input) =>
            new ContactPersonViewModel
            {
                Id = input.Id, 
                Name = input.Name,
                Surname = input.Surname,
                Address = input.Address,
                PhoneNumber = input.PhoneNumber,
                Email = input.Email,
                AlternativePhoneNumber = input.AlternativePhoneNumber,
                CaseId = input.CaseId,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy,
            };

        public static implicit operator ContactPersonViewModel(ClientContactPerson input) =>
            new ContactPersonViewModel
            {
                Id = input.Id,
                Name = input.Name,
                Surname = input.Surname,
                Address = input.Address,
                PhoneNumber = input.PhoneNumber,
                Email = input.Email,
                AlternativePhoneNumber = input.AlternativePhoneNumber,
                ClientId = input.ClientId,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy,
            };

        public static implicit operator CaseContactPerson(ContactPersonViewModel input) =>
            new CaseContactPerson
            {
                Id = input.Id, 
                Name = input.Name,
                Surname = input.Surname,
                Address = input.Address,
                PhoneNumber = input.PhoneNumber,
                Email = input.Email,
                AlternativePhoneNumber = input.AlternativePhoneNumber,
                CaseId = input.CaseId,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy,
            };

        public static implicit operator ClientContactPerson(ContactPersonViewModel input) =>
            new ClientContactPerson
            {
                Id = input.Id,
                Name = input.Name,
                Surname = input.Surname,
                Address = input.Address,
                PhoneNumber = input.PhoneNumber,
                Email = input.Email,
                AlternativePhoneNumber = input.AlternativePhoneNumber,
                ClientId = input.ClientId,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy,
            };
    }
}
