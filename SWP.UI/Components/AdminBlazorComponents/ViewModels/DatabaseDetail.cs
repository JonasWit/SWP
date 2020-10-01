using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents.ViewModels
{
    public class DatabaseDetail
    {
        public string Profile { get; set; }

        public int Clients { get; set; }
        public int ClientJobs { get; set; }
        public int ClientCashMovements { get; set; }
        public int ClientTimeRecord { get; set; }

        public int Cases { get; set; }
        public int Notes { get; set; }
        public int Reminders { get; set; }
    }
}
