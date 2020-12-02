﻿using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;

namespace SWP.Application.LegalSwp.Notes
{
    [TransientService]
    public class GetNote
    {
        private readonly ILegalManager legalSwpManager;
        public GetNote(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;
        public Note Get(int id) => legalSwpManager.GetNote(id);
    }
}
