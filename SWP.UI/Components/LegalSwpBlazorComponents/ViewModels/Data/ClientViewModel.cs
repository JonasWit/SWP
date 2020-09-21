using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data
{
    public class ClientViewModel
    {
        public int Id { get; set; }
        public string IdStr => Id.ToString();

        public string Name { get; set; }
        public bool Active { get; set; }

        public string ProfileClaim { get; set; }

        public string Address { get; set; }

        public CaseViewModel SelectedCase { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public List<CaseViewModel> Cases { get; set; }

        public List<ClientJobViewModel> Jobs { get; set; }

        public List<CashMovementViewModel> CashMovements { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }

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
                Cases = input.Cases == null ? new List<CaseViewModel>() : input.Cases.Select(x => (CaseViewModel)x).ToList(),
                Jobs = input.Jobs == null ? new List<ClientJobViewModel>() : input.Jobs.Select(x => (ClientJobViewModel)x).ToList(),
                CashMovements = input.CashMovements == null ? new List<CashMovementViewModel>() : input.CashMovements.Select(x => (CashMovementViewModel)x).ToList(),
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy
            };
    }
}
