using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.ClientDetails.Actions
{
    public class ContactSelectedAction : IAction
    {
        public const string ContactSelected = "CONTACT_SELECTED";
        public string Name => ContactSelected;

        public object Arg { get; set; }
    }
}
