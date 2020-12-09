using SWP.Domain.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Domain.Models.Portal.Communication
{
    public class ClientRequest : BaseModel
    {
        public string Subject { get; set; }
        public List<ClientRequestMessage> Messages { get; set; }
    }
}
