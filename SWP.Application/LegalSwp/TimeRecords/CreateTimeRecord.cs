using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.TimeRecords
{
    [TransientService]
    public class CreateTimeRecord
    {
        private readonly ILegalSwpManager legalSwpManager;
        public CreateTimeRecord(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<TimeRecord> Create(int clientId, string profile, Request request) =>
            legalSwpManager.CreateTimeRecord(clientId, profile, new TimeRecord
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
