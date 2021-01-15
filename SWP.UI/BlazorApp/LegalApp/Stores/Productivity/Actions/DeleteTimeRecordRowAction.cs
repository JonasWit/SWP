using SWP.UI.Components.ViewModels.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Productivity.Actions
{
    public class DeleteTimeRecordRowAction : IAction
    {
        public const string DeleteTimeRecordRow = "DELETE_TIME_RECORD_ROW";
        public string Name => DeleteTimeRecordRow;

        public TimeRecordViewModel Arg { get; set; }
    }
}
