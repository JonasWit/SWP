using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data
{
    public abstract class ViewModelBase
    {
        public int Id { get; set; }
        public string IdString => Id.ToString();
    }
}
