using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalAppBlazorComponents.Dialogs
{
    public partial class GenericDialogPopup : DialogBase
    {
        public async Task Agree(DialogResult dialogResult)
        {
            if (dialogResult == null)
            {
                DialogService.Close();
                return;
            }

            if (!dialogResult.Allowed)
            {
                DialogService.Close();
                return;
            }

            if (dialogResult.TaskToExecuteAsync != null)
            {
                await dialogResult.TaskToExecuteAsync.Invoke();
                DialogService.Close();
                return;
            }

            if (dialogResult.TaskToExecute != null)
            {
                dialogResult.TaskToExecute.Invoke();
                DialogService.Close();
                return;
            }

            DialogService.Close();
        }

        public Task Disagree(DialogResult dialogResult)
        {
            DialogService.Close();
            return Task.CompletedTask;
        }
    }
}
