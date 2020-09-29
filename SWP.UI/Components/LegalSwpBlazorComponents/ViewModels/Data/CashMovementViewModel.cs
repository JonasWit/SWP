using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data
{
    public class CashMovementViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public int ClientId { get; set; }

        public DateTime EventDate { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }

        public static implicit operator CashMovementViewModel(CashMovement input) =>
            new CashMovementViewModel
            {
                Id = input.Id,
                Amount = input.Amount,
                Name = input.Name,
                ClientId = input.ClientId,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy,
                EventDate = input.EventDate
            };
    }
}
