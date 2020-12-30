using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.TimeRecords
{
    [TransientService]
    public class CreateTimeRecord : LegalActionsBase
    {
        public CreateTimeRecord(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<TimeRecord> Create(int clientId, string profile, Request request) =>
            _legalManager.CreateTimeRecord(clientId, profile, new TimeRecord
            {
                Lawyer = request.Lawyer,
                Rate = request.Rate,
                Name = request.Name,
                Hours = request.RecordedHours,
                Minutes = request.RecordedMinutes,
                Description = request.Description,
                EventDate = request.EventDate,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                UpdatedBy = request.UpdatedBy,
                CreatedBy = request.UpdatedBy
            });

        public class Request
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Lawyer { get; set; }
            public double Rate { get; set; }
            public int RecordedHours { get; set; } = 0;
            public int RecordedMinutes { get; set; } = 0;
            public DateTime EventDate { get; set; } = DateTime.Now;
            public TimeSpan RecordedTime => new TimeSpan(RecordedHours, RecordedMinutes, 0);
            public string UpdatedBy { get; set; }
        }
    }
}
