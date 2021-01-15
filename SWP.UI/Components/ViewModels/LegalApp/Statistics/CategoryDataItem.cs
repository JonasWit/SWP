using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.ViewModels.LegalApp.Statistics
{
    public class CategoryDataItem
    {
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public double Number { get; set; } = 0;
    }
}
