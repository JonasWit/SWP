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
                Name = request.Name,
                RecordedTime = request.RecordedTime.TimeOfDay,
                Description = request.Description,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                UpdatedBy = request.UpdatedBy,
                CreatedBy = request.UpdatedBy
            });

        public class Request
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime RecordedTime { get; set; }
            public string UpdatedBy { get; set; }
        }
    }
}
