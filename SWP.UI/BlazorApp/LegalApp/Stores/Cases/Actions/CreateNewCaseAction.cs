using SWP.Application.LegalSwp.Cases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class CreateNewCaseAction : IAction
    {
        public const string CreateNewCase = "CREATE_NEW_CASE";
        public string Name => CreateNewCase;

        public CreateCase.Request Request { get; set; }
    }
}
