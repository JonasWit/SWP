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
                AutoRenewal = request.AutoRenewal,
                RequestorName = request.RequestorName
            };

            changes += await _portalManager.CreateRequest(newRequestEntity);

            var newRequesrMessageEntity = new ClientRequestMessage
            {
                AuthorName = request.RequestMessage.AuthorName,
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
                AuthorName = request.AuthorName,
                Created = DateTime.Now,
                CreatedBy = request.AuthorName,
                Message = request.Message,
                Updated = DateTime.Now,
                UpdatedBy = request.AuthorName
            }, reuqestId);
        

        public class Request
        {
            public string RequestorName { get; set; }
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
            public string AuthorName { get; set; }
            public string Message { get; set; }
        }
    }
}
