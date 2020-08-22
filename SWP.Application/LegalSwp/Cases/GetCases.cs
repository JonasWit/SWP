using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.LegalSwp.Cases
{
    [TransientService]
    public class GetCases
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetCases(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Case Get(string profile) => legalSwpManager.GetCases(profile, x => x);

    }
}
