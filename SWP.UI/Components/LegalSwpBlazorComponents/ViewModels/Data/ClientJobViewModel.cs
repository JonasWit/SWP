using SWP.Domain.Models.SWPLegal;
using System;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data
{
    public class ClientJobViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public int Priority { get; set; }

        public int ClientId { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }

        public static implicit operator ClientJobViewModel(ClientJob input) =>
            new ClientJobViewModel
            {
                Id = input.Id,
                Name = input.Name,
                Description = input.Description,
                Active = input.Active,
                Priority = input.Priority,
                ClientId = input.ClientId,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy
            };
    }
}
