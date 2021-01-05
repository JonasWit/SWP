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
    public class UpdateRequest : PortalManagerBase
    {
        public UpdateRequest(IPortalManager portalManager) : base(portalManager) { }

        public Task<int> Update(ClientRequest clientRequest) => _portalManager.UpdateRequest(clientRequest);
    }
}
