using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class OnUpdateNoteRowAction : IAction
    {
        public const string OnUpdateNoteRow = "ON_UPDATE_NOTE_ROW";
        public string Name => OnUpdateNoteRow;

        public NoteViewModel Arg { get; set; }
    }
}
