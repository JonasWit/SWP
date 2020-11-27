using SWP.Application.LegalSwp.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class CreateNewNoteAction : IAction
    {
        public const string CreateNewNote = "CREATE_NEW_NOTE";
        public string Name => CreateNewNote;

        public CreateNote.Request Request { get; set; }
    }
}
