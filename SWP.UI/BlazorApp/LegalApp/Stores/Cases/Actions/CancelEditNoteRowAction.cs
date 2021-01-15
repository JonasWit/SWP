using SWP.UI.Components.ViewModels.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class CancelEditNoteRowAction : IAction
    {
        public const string CancelEditNoteRow = "CANCEL_EDIT_NOTE_ROW";
        public string Name => CancelEditNoteRow;

        public NoteViewModel Arg { get; set; }
    }
}
