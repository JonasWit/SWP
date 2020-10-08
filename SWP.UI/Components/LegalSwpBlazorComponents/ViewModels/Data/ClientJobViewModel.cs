using SWP.Domain.Models.SWPLegal;
using System;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data
{
    public class ClientJobViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public int Priority { get; set; }

        public int ClientId { get; set; }

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
