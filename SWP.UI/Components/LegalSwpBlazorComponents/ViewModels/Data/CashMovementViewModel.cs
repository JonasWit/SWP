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

        public int CustomerId { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }

        public static implicit operator CashMovementViewModel(CashMovement input) =>
            new CashMovementViewModel
            {
                Id = input.Id,
                Name = input.Name,
                CustomerId = input.CustomerId,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy
            };
    }
}
