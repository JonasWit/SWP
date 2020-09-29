using Microsoft.AspNetCore.Components;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.SchedulerInnerComponents
{
    public partial class AddReminderPage
    {
        [Parameter]
        public DateTime Start { get; set; }
        [Parameter]
        public DateTime End { get; set; }

        public ReminderViewModel model = new ReminderViewModel();
        public GeneralViewModel generalVm = new GeneralViewModel();

        protected override void OnParametersSet()
        {
            model.Start = Start;
            model.End = End;
        }

        public void OnSubmit(ReminderViewModel model)
        {
            DialogService.Close(model);
        }
    }
}
