using SWP.UI.Components.ViewModels.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.ClientDetails.Actions
{
    public class EditContactRowAction : IAction
    {
        public const string EditContactRow = "EDIT_CONTACT_ROW";
        public string Name => EditContactRow;

        public ContactPersonViewModel Arg { get; set; }
    }
}
