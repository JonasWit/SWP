using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.ViewModels.LegalApp.Statistics
{
    public class ClientData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<DateDataItem> DataByDate { get; set; } = new List<DateDataItem>();
        public List<CategoryDataItem> DataByCategory { get; set; } = new List<CategoryDataItem>();

    }
}
