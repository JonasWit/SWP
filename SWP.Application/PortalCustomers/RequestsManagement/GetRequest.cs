using SWP.Domain.Enums;
using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Models.Portal.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.PortalCustomers.RequestsManagement
{
    [TransientService]
    public class GetRequest : PortalManagerBase
    {
        public GetRequest(IPortalManager portalManager) : base(portalManager)
        {
        }

        public ClientRequest GetRequestWithMessages(int requestId) => _portalManager.GetRequestWithMessages(requestId);

        public List<ClientRequest> GetRequestsForClient(string userId) => _portalManager.GetRequestsForClient(userId);

        public List<ClientRequest> GetRequests(RequestStatus status) => _portalManager.GetRequests(x => x, x => x.Status.Equals(status.ToString()));

        public List<ClientRequest> GetRequests() => _portalManager.GetRequests();
    }
}
