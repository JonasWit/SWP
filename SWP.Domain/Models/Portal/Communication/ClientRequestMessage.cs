using SWP.Domain.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Domain.Models.Portal.Communication
{
    //todo:update DB
    public class ClientRequestMessage : BaseModel
    {
        public string Message { get; set; }

        public int ClientRequestId { get; set; }
        public ClientRequest ClientRequest { get; set; }
    }
}
