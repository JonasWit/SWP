using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.LegalSwp.TimeRecords
{
    [TransientService]
    public class UpdateTimeRecord
    {
        private readonly ILegalSwpManager legalSwpManager;
        public UpdateTimeRecord(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;
    }
}
