﻿using SWP.Domain.Infrastructure.Portal;
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


    }
}
