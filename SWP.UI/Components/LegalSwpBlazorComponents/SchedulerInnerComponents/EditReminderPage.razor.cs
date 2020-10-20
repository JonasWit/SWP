using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.SchedulerInnerComponents
{
    public partial class EditReminderPage : ComponentBase
    {
        [Parameter]
        public ReminderViewModel Reminder { get; set; }
        [Inject]
        public DialogService DialogService { get; set; }

        public ReminderViewModel model = new ReminderViewModel();
        public GeneralViewModel generalVm = new GeneralViewModel();

        protected override void OnParametersSet()
        {
            model = Reminder;
        }

        public void OnSubmit(ReminderViewModel model)
        {
            DialogService.Close(model);
        }

        public void Delete()
        {
            Reminder.Active = false;
            DialogService.Close(Reminder);
        }

        public void Close()
        {
            DialogService.Close();
        }
    }
}
