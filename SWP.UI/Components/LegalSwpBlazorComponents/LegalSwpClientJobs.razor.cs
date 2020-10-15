using Microsoft.AspNetCore.Components;
using SWP.UI.Components.LegalSwpBlazorComponents.App;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpClientJobs
    {
        [Parameter]
        public LegalBlazorApp App { get; set; }
        [Parameter]
        public EventCallback<LegalBlazorApp> AppChanged { get; set; }

        public string ArchvizedJobsFilterValue;

        public bool showFormVisible = false;
        public void ShowHideForm() => showFormVisible = !showFormVisible;

        public bool addClientformVisible = false;
        public void ShowHideClientFormI() => addClientformVisible = !addClientformVisible;

    }
}
