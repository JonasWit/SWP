using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data
{
    public class ClientViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public string ProfileClaim { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public static implicit operator ClientViewModel(Client input) =>
            new ClientViewModel
            { 
                Id = input.Id,
                Name = input.Name,
                Active = input.Active,
                ProfileClaim = input.ProfileClaim,
                Address = input.Address,
                Email = input.Email,
                PhoneNumber = input.PhoneNumber,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy
            };
    }
}
