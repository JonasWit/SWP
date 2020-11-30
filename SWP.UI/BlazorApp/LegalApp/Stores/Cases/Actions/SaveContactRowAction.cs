using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class SaveContactRowAction : IAction
    {
        public const string SaveContactRow = "SAVE_CONTACT_ROW";
        public string Name => SaveContactRow;

        public ContactPersonViewModel Arg { get; set; }
    }
}
