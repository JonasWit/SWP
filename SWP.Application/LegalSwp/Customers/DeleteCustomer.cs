using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Customers
{
    [TransientService]
    public class DeleteCustomer
    {
        private readonly ILegalSwpManager legalSwpManager;
        public DeleteCustomer(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<int> Delete(int id) => legalSwpManager.DeleteCustomer(id);
        public Task<int> Delete(string profile) => legalSwpManager.DeleteProfileCustomers(profile);
    }
}
