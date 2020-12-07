using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Communication.Actions
{
    public class ClearSelectionAction : IAction
    {
        public const string ClearSelection = "CLEAR_SELECTION";
        public string Name => ClearSelection;
    }
}
