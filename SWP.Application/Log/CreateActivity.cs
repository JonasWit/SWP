using SWP.Application.PortalCustomers;
using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Models.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.Log
{
    [TransientService]
    public class CreateActivity : PortalCustomersBase
    {
        public CreateActivity(IPortalManager portalManager) : base(portalManager)
        {
        }

        public Task<int> Create(Request request) =>
            _portalManager.CreateActivity(new Activity
            {
                Action = request.Action,
                Message = request.Message,
                TimeStamp = DateTime.Now,
                UserId = request.UserId,
                UserName = request.UserName
            });

        public class Request
        {
            public int Id { get; set; }
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Message { get; set; }
            public string Action { get; set; }
        }

         


    }
}
