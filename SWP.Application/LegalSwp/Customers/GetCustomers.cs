using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Customers
{
    [TransientService]
    public class GetCustomers
    {
        private readonly ILegalSwpManager legalSwpManager;
        public GetCustomers(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public List<Customer> GetCustomersWithData(string claim) => legalSwpManager.GetCustomers(claim, x => x);
        public List<Customer> GetCustomersWithoutData(string claim) => legalSwpManager.GetCustomersWithoutCases(claim, x => x);
    }
}
