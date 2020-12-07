using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Productivity.Actions
{
    public class CancelTimeRecordEditAction : IAction
    {
        public const string CancelTimeRecordEdit = "CANCEL_TIME_RECORD_EDIT";
        public string Name => CancelTimeRecordEdit;

        public TimeRecordViewModel Arg { get; set; }
    }
}
