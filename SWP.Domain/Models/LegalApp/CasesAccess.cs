using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Domain.Models.LegalApp
{
    //todo: add to context
    public class CasesAccess
    {
        public string UserId { get; set; }
        public int CaseId { get; set; }
    }
}
