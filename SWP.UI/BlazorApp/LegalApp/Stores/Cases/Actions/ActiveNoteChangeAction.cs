using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class ActiveNoteChangeAction : IAction
    {
        public const string ActiveNoteChange = "ACTIVE_NOTE_CHANGE";
        public string Name => ActiveNoteChange;

        public object Arg { get; set; }
    }
}
