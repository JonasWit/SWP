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

        public Task<ClientRequest> GetRequestWithMessages(int requestId) => 
            _portalManager.GetRequestWithMessages(requestId);

        public Task<List<ClientRequest>> GetRequestsForClient(string userId) => 
            _portalManager.GetRequestsForClient(userId);
    }
}
