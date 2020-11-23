using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Archive.Actions
{
    public class DeleteSelectedCaseAction : IAction
    {
        public const string DeleteSelectedCase = "DELETE_SELECTED_CASE";
        public string Name => DeleteSelectedCase;
    }
}
