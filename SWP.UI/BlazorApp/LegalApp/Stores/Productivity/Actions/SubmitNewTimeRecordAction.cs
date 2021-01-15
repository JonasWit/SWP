using SWP.Application.LegalSwp.TimeRecords;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Productivity.Actions
{
    public class SubmitNewTimeRecordAction : IAction
    {
        public const string SubmitNewTimeRecord = "SUBMIT_NEW_TIME_RECORD";
        public string Name => SubmitNewTimeRecord;

        public CreateTimeRecord.Request Arg { get; set; }
    }
}
