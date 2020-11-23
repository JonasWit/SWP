using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Archive.Actions
{
    public class RecoverSelectedCaseAction : IAction
    {
        public const string RecoverSelectedCase = "RECOVER_SELECTED_CASE";
        public string Name => RecoverSelectedCase;
    }
}
