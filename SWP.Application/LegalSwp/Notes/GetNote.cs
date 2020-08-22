using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.LegalSwp.Notes
{
    [TransientService]
    public class GetNote
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetNote(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;
        public Note Get(int id) => legalSwpManager.GetNote(id);
    }
}
