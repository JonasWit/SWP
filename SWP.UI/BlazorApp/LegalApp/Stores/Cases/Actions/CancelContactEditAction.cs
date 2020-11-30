using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class CancelContactEditAction : IAction
    {
        public const string CancelContactEdit = "CANCEL_CONTACT_EDIT";
        public string Name => CancelContactEdit;

        public ContactPersonViewModel Arg { get; set; }
    }
}
