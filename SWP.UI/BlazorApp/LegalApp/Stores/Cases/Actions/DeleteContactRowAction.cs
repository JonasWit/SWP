using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class DeleteContactRowAction : IAction
    {
        public const string DeleteContactRow = "DELETE_CONTACT_ROW";
        public string Name => DeleteContactRow;

        public ContactPersonViewModel Arg { get; set; }
    }
}
