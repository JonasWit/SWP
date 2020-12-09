using SWP.Domain.Infrastructure.Portal;
using SWP.Domain.Models.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.PortalCustomers.LicenseManagement
{
    [TransientService]
    public class CreateLicense : PortalCustomersBase
    {
        public CreateLicense(IPortalManager portalManager) : base(portalManager) { }

        public Task<UserLicense> Create(Request request)
        {
            return _portalManager.CreateLicense(new UserLicense 
            { 
                UserId = request.UserId,
                Created = DateTime.Now,
                CreatedBy = request.CreatedBy,
                Updated = DateTime.Now,
                UpdatedBy = request.CreatedBy,
                Application = request.Product,
                Type = request.Type,
                RelatedUsers = request.RelatedUsers,
                ValidTo = request.ValidTo
            });
        }
    
        public class Request
        {
            public string UserId { get; set; }
            public string Product { get; set; }
            public string Type { get; set; }
            public int RelatedUsers { get; set; }
            public DateTime ValidTo { get; set; }
            public string CreatedBy { get; set; }
        }
    }
}
