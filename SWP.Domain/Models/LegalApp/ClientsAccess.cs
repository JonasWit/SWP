using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Domain.Models.LegalApp
{
    //todo: add to context
    public class ClientsAccess
    {
        public string UserId { get; set; }
        public int ClientId { get; set; }
    }
}
