using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.LegalSwp.Notes
{
    [TransientService]
    public class GetNotes
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetNotes(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;



    }
}
