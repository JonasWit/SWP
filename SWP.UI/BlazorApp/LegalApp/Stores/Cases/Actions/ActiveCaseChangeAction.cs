using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class ActiveCaseChangeAction : IAction
    {
        public const string ActiveCaseChange = "ACTIVE_CASE_CHANGE_ACTION";
        public string Name => ActiveCaseChange;

        public object Arg { get; set; }
    }
}
