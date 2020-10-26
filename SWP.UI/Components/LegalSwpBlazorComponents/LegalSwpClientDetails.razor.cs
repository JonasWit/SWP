using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.UI.Components.LegalSwpBlazorComponents.App;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpClientDetails
    {

        [Inject]
        public GeneralViewModel Gvm { get; set; }

        [Inject]
        public TooltipService TooltipService { get; set; }

        public bool addClientformVisible = false;
        [Inject]
        public LegalBlazorApp App { get; set; }

        public bool contactsListInfoVisible = false;
        public void ShowHideContactsI() => contactsListInfoVisible = !contactsListInfoVisible;

        public bool showContactsListVisible = false;
        public void ShowHideContactsList() => showContactsListVisible = !showContactsListVisible;

        
    }
}
