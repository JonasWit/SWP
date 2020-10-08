using SWP.Domain.Models.SWPLegal;

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
        public int ClientId { get; set; }
        public int CaseId { get; set; }

        public static implicit operator ContactPersonViewModel(ContactPerson input) =>
            new ContactPersonViewModel
            {
                Id = input.Id, 
                Name = input.Name,
                Surname = input.Surname,
                Address = input.Address,
                PhoneNumber = input.PhoneNumber,
                AlternativePhoneNumber = input.AlternativePhoneNumber,
                ClientId = input.ClientId,
                CaseId = input.CaseId,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy,
            };

        public static implicit operator ContactPerson(ContactPersonViewModel input) =>
            new ContactPersonViewModel
            {
                Id = input.Id, 
                Name = input.Name,
                Surname = input.Surname,
                Address = input.Address,
                PhoneNumber = input.PhoneNumber,
                AlternativePhoneNumber = input.AlternativePhoneNumber,
                ClientId = input.ClientId,
                CaseId = input.CaseId,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy,
            };
    }
}
