using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.LegalSwp.TimeRecords
{
    [TransientService]
    public class GetTimeRecords
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetTimeRecords(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;
    }
}
