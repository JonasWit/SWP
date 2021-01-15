using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.ViewModels.LegalApp
{
    public abstract class ViewModelBase
    {
        public int Id { get; set; }
        public string IdString => Id.ToString();

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedDescription => $"Ostatnia zamina {Updated} przez {UpdatedBy}";
        public string CreatedDescription => $"Stworono {Created}";
    }
}
