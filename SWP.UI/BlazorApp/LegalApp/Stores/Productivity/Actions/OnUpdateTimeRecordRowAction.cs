using SWP.UI.Components.ViewModels.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Productivity.Actions
{
    public class OnUpdateTimeRecordRowAction : IAction
    {
        public const string OnUpdateTimeRecordRow = "ON_UPDATE_TIME_RECORD_ROW";
        public string Name => OnUpdateTimeRecordRow;

        public TimeRecordViewModel Arg { get; set; }
    }
}
