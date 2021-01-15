using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalAppBlazorComponents.Dialogs
{
    public class DialogResult
    {
        public bool Allowed { get; set; }
        public Func<Task> TaskToExecuteAsync { get; set; }
        public Action TaskToExecute { get; set; }
    }
}
