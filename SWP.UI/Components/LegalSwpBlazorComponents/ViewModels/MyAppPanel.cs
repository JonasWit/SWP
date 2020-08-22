using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels
{
    [UITransientService]
    public class MyAppPanel
    {
        public LegalSwpApp App { get; private set; }
        public void Initialize(LegalSwpApp app) => App = app;





    }
}
