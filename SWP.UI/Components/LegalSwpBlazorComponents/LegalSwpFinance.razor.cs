using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.UI.Components.LegalSwpBlazorComponents.App;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpFinance
    {
        [Inject]
        public LegalBlazorApp App { get; set; }

        [Inject]
        public GeneralViewModel Gvm { get; set; }

        [Inject]
        public TooltipService TooltipService { get; set; }

        public bool showFirstSection = false;
        public void ShowHideFirstSection() => showFirstSection = !showFirstSection;

        public bool showSecondSection = false;
        public void ShowHideSecondSection() => showSecondSection = !showSecondSection;

        public bool infoBoxVisibleI = false;
        public void ShowHideInfoBoxI() => infoBoxVisibleI = !infoBoxVisibleI;

        public bool infoBoxVisibleII = false;
        public void ShowHideInfoBoxII() => infoBoxVisibleII = !infoBoxVisibleII;

        public bool infoBoxVisibleIII = false;
        public void ShowHideInfoBoxIII() => infoBoxVisibleIII = !infoBoxVisibleIII;
    }
}
