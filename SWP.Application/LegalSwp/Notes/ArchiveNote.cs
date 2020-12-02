﻿using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Notes
{
    [TransientService]
    public class ArchiveNote
    {
        private readonly ILegalManager legalSwpManager;
        public ArchiveNote(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<int> ArchivizeNore(int noteId, string user) => legalSwpManager.ArchivizeNote(noteId, user);
        public Note RecoverClientJob(int noteId, string user) => legalSwpManager.RecoverNote(noteId, user);
    }
}
