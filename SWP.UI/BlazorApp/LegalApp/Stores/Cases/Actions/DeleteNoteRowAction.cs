using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class DeleteNoteRowAction : IAction
    {
        public const string DeleteNoteRow = "DELETE_NOTE_ROW";
        public string Name => DeleteNoteRow;

        public NoteViewModel Arg { get; set; }
    }
}
