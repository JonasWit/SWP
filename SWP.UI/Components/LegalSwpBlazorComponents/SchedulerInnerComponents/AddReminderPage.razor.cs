using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.SchedulerInnerComponents
{
    public partial class AddReminderPage : ComponentBase
    {
        [Parameter]
        public DateTime Start { get; set; }
        [Parameter]
        public DateTime End { get; set; }
        [Inject]
        public DialogService DialogService { get; set; }

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

        public void Close()
        {
            DialogService.Close();
        }
    }
}
