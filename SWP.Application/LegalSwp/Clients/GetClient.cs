using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.LegalSwp.Clients
{
    [TransientService]
    public class GetClient
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetClient(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Client Get(int id) => legalSwpManager.GetClient(id);
    }
}
