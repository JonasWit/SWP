using SWP.UI.Components.ViewModels.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Productivity.Actions
{
    public class SaveTimeRecordRowAction : IAction
    {
        public const string SaveTimeRecordRow = "SAVE_TIME_RECORD_ROW";
        public string Name => SaveTimeRecordRow;

        public TimeRecordViewModel Arg { get; set; }
    }
}
