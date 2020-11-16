using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.TimeRecords
{
    [TransientService]
    public class UpdateTimeRecord
    {
        private readonly ILegalManager legalSwpManager;
        public UpdateTimeRecord(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<TimeRecord> Update(Request request)
        {
            var tr = legalSwpManager.GetTimeRecord(request.Id);

            tr.Name = request.Name;
            tr.Lawyer = request.Lawyer;
            tr.Rate = request.Rate;
            tr.Description = request.Description;
            tr.Hours = request.RecordedHours;
            tr.Minutes = request.RecordedMinutes;
            tr.EventDate = request.EventDate;
            tr.Updated = request.Updated;
            tr.UpdatedBy = request.UpdatedBy;

            return legalSwpManager.UpdateTimeRecord(tr);
        }

        public class Request
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Lawyer { get; set; }
            public double Rate { get; set; }
            public string Description { get; set; }
            public int RecordedHours { get; set; } = 0;
            public int RecordedMinutes { get; set; } = 0;
            public DateTime EventDate { get; set; } = DateTime.Now;
            public DateTime Updated { get; set; }
            public string UpdatedBy { get; set; }
        }
    }
}
