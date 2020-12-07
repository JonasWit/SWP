using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Productivity.Actions
{
    public class ActiveTimeRecordChangeAction : IAction
    {
        public const string ActiveTimeRecordChange = "ACTIVE_TIME_RECORD_CHANGE";
        public string Name => ActiveTimeRecordChange;

        public object Arg { get; set; }
    }
}
