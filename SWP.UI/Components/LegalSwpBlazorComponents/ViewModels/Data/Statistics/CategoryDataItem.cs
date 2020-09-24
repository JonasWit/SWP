using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data.Statistics
{
    public class CategoryDataItem
    {
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public double Number { get; set; } = 0;
    }
}
