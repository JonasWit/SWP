using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Models.Portal.Communication;
using System;
using System.Threading.Tasks;

namespace SWP.Application.PortalCustomers.RequestsManagement
{
    [TransientService]
    public class CreateRequest : PortalManagerBase
    {
        public CreateRequest(IPortalManager portalManager) : base(portalManager) { }

        public async Task<int> Create(Request request)
        {
            var changes = 0;

            var newRequestEntity = new ClientRequest
            {
                Application = request.Application,
                Created = request.Created,
                CreatedBy = request.CreatedBy,
                Updated = request.Created,
                UpdatedBy = request.CreatedBy,
                Reason = request.Reason,
                LicenseMonths = request.LicenseMonths,
                RelatedUsers = request.RelatedUsers,
                Status = request.Status,
                AutoRenewal = request.AutoRenewal
            };

            changes += await _portalManager.CreateRequest(newRequestEntity);

            var newRequesrMessageEntity = new ClientRequestMessage
            {
                AuthorId = request.RequestMessage.AuthorId,
                Created = request.Created,
                CreatedBy = request.CreatedBy,
                Updated = request.Created,
                UpdatedBy = request.CreatedBy,
                Message = request.RequestMessage.Message
            };

            changes += await _portalManager.CreateRequestMessage(newRequesrMessageEntity, newRequestEntity.Id);

            return changes;
        }

        public Task<int> Create(RequestMessage request, int reuqestId) =>       
            _portalManager.CreateRequestMessage(new ClientRequestMessage
            {
                AuthorId = request.AuthorId,
                Created = DateTime.Now,
                CreatedBy = request.AuthorId,
                Message = request.Message,
                Updated = DateTime.Now,
                UpdatedBy = request.AuthorId
            }, reuqestId);
        

        public class Request
        {
            public int Reason { get; set; }
            public int Application { get; set; }
            public int Status { get; set; }
            public int LicenseMonths { get; set; }
            public int RelatedUsers { get; set; }
            public bool AutoRenewal { get; set; }

            public DateTime Created { get; set; }
            public string CreatedBy { get; set; }

            public RequestMessage RequestMessage { get; set; } = new RequestMessage();
        }

        public class RequestMessage
        {
            public string AuthorId { get; set; }
            public string Message { get; set; }
        }
    }
}
