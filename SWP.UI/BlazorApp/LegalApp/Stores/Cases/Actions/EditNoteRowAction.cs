using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class EditNoteRowAction : IAction
    {
        public const string EditNoteRow = "EDIT_NOTE_ROW";
        public string Name => EditNoteRow;

        public NoteViewModel Arg { get; set; }
    }
}
