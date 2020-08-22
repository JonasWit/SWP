using SWP.Domain.Models.SWPLegal;
using System;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data
{
    public class CustomerJobViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public int Priority { get; set; }

        public int CustomerId { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }

        public static implicit operator CustomerJobViewModel(CustomerJob input) =>
            new CustomerJobViewModel
            {
                Id = input.Id,
                Name = input.Name,
                Description = input.Description,
                Active = input.Active,
                Priority = input.Priority,
                CustomerId = input.CustomerId,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy
            };
    }
}
