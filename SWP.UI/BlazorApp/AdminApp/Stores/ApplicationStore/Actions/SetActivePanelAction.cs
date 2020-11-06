using SWP.UI.BlazorApp.AdminApp.Stores.Enums;
using SWP.UI.BlazorApp.LegalApp.Stores.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.ApplicationStore.Actions
{
    public class SetActivePanelAction : IAction
    {
        public const string SetActivePanel = "SetActivePanel";
        public string Name => SetActivePanel;
        public AdminAppPanels ActivePanel { get; set; }
    }
}
