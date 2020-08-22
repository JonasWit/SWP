using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.LegalSwp.Customers
{
    [TransientService]
    public class GetCustomer
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetCustomer(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;


        public Customer Get(int id, string claim) => legalSwpManager.GetCustomer(id, claim, x => x);
    }
}
