using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.LegalSwp.TimeRecords
{
    [TransientService]
    public class GetTimeRecord
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetTimeRecord(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public TimeRecord Get(int id) => legalSwpManager.GetTimeRecord(id);
    }
}
