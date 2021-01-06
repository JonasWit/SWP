using SWP.Domain.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Domain.Models.Portal.Communication
{
    public class ClientRequest : BaseModel
    {
        public string RequestorId { get; set; }
        public int Reason { get; set; }
        public int Application { get; set; }
        public int Status { get; set; }
        public int LicenseMonths { get; set; }
        public int RelatedUsers { get; set; }

        public List<ClientRequestMessage> Messages { get; set; }
    }
}
