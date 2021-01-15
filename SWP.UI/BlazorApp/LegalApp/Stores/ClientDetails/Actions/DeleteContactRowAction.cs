using SWP.UI.Components.ViewModels.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.ClientDetails.Actions
{
    public class DeleteContactRowAction : IAction
    {
        public const string DeleteContactRow = "DELETE_CONTACT_ROW";
        public string Name => DeleteContactRow;

        public ContactPersonViewModel Arg { get; set; }
    }
}
