using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.CashMovements
{
    [TransientService]
    public class UpdateCashMovement
    {
        private readonly ILegalSwpManager legalSwpManager;
        public UpdateCashMovement(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<CashMovement> Update(Request request)
        {
            var c = legalSwpManager.GetCashMovement(request.Id);

            c.Name = request.Name;
            c.Amount = request.Amount;
            c.Updated = request.Updated;
            c.UpdatedBy = request.UpdatedBy;
            c.EventDate = request.EventDate;

            return legalSwpManager.UpdateCashMovement(c);
        }

        public class Request
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Amount { get; set; }
            public DateTime Updated { get; set; }
            public DateTime EventDate { get; set; } = DateTime.Now;
            public string UpdatedBy { get; set; }
        }
    }
}
