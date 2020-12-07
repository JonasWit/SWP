using SWP.Application.LegalSwp.TimeRecords;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Productivity.Actions
{
    public class SubmitNewTimeRecordAction : IAction
    {
        public const string SubmitNewTimeRecord = "SUBMIT_NEW_TIME_RECORD";
        public string Name => SubmitNewTimeRecord;

        public CreateTimeRecord.Request Arg { get; set; }
    }
}
