using SWP.Domain.Models.Portal.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.PortalBlazorComponents.Requests.ViewModels
{
    public class RequestMessageViewModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public string AuthorName { get; set; }
        public string Message { get; set; }

        public static implicit operator RequestMessageViewModel(ClientRequestMessage input) =>
            new RequestMessageViewModel
            {
                Id = input.Id,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy,
                AuthorName = input.AuthorName,
                Message = input.Message
            };
    }
}
