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
        [MaxLength(100)]
        public string RequestType { get; set; }
        [MaxLength(100)]
        public string HelpType { get; set; }
        [MaxLength(100)]
        public string RelatedApp { get; set; }
        public DateTime DateParam { get; set; }
        public int RelatedUsers { get; set; }
        public List<ClientRequestMessage> Messages { get; set; }
    }
}
