using SWP.UI.Components.ViewModels.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class SaveNoteRowAction : IAction
    {
        public const string SaveNoteRow = "SAVE_NOTE_ROW";
        public string Name => SaveNoteRow;

        public NoteViewModel Arg { get; set; }
    }
}
