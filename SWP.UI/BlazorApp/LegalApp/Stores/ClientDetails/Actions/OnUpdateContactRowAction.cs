using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.ClientDetails.Actions
{
    public class OnUpdateContactRowAction : IAction
    {
        public const string OnUpdateContactRow = "ON_UPDATE_CONTACT_ROW";
        public string Name => OnUpdateContactRow;

        public ContactPersonViewModel Arg { get; set; }
    }
}
