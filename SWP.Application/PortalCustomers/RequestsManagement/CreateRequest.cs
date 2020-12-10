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
    public class CreateRequest : PortalManagerBase
    {
        public CreateRequest(IPortalManager portalManager) : base(portalManager)
        {
        }

        public Task<ClientRequest> Create(ClientRequest clientRequest) => 
            _portalManager.CreateRequest(clientRequest);

        public Task<ClientRequestMessage> Create(ClientRequestMessage clientRequestMessage, int reuqestId) => 
            _portalManager.CreateRequestMessage(clientRequestMessage, reuqestId);
    }
}
