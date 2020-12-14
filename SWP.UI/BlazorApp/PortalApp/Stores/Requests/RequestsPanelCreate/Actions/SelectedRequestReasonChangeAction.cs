using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanelCreate.Actions
{
    public class SelectedRequestReasonChangeAction : IAction
    {
        public const string SelectedRequestSubjectChange = "SELECTED_REQUEST_SUBJECT_CHANGE";
        public string Name => SelectedRequestSubjectChange;

        public int? Arg { get; set; }
    }
}
