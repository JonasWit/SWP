using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.Dialogs
{
    public class DialogBase : ComponentBase
    {
        [Parameter]
        public string Title { get; set; }
        [Parameter]
        public string Description { get; set; }
        [Parameter]
        public Func<Task> TaskToExecuteAsync { get; set; }
        [Parameter]
        public Action TaskToExecute { get; set; }
        [Inject]
        public DialogService DialogService { get; set; }
    }
}
