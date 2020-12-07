using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Productivity.Actions
{
    public class EditTimeRecordRowAction : IAction
    {
        public const string EditTimeRecordRow = "EDIT_TIME_RECORD_ROW";
        public string Name => EditTimeRecordRow;

        public TimeRecordViewModel Arg { get; set; }
    }
}
