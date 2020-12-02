using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Domain.Utilities
{
    public class ManagerActionResult : Exception
    {
        public bool Success { get; set; }
        public string CustomMessage { get; set; }
    }
}
