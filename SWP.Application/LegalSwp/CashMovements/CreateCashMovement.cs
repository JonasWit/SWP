using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.CashMovements
{
    [TransientService]
    public class CreateCashMovement
    {
        private readonly ILegalManager legalSwpManager;
        public CreateCashMovement(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<CashMovement> Create(int clientId, string profile, Request request) =>
            legalSwpManager.CreateCashMovement(clientId, profile, new CashMovement
            {
                Name = request.Name,
                Amount = request.Expense ? request.ExpenseAmount : request.ResultAmount,
                EventDate = request.EventDate,
                Expense = request.Expense,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                UpdatedBy = request.UpdatedBy,
                CreatedBy = request.UpdatedBy
            });

        public class Request
        {
            public string Name { get; set; }
            public double Amount { get; set; }
            public bool Expense { get; set; }
            public int CashFlowDirection { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime EventDate { get; set; } = DateTime.Now;
            public double ResultAmount => CashFlowDirection == 0 ? Math.Abs(Amount) * -1 : Math.Abs(Amount);
            public double ExpenseAmount => (Math.Abs(Amount) * (-1));
        }
    }
}
