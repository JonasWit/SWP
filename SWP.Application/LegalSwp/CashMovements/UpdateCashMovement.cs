﻿using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.CashMovements
{
    [TransientService]
    public class UpdateCashMovement : LegalActionsBase
    {
        public UpdateCashMovement(ILegalManager legalManager) : base(legalManager)
        {
        }

        public Task<CashMovement> Update(Request request)
        {
            var c = _legalManager.GetCashMovement(request.Id);
            double amount = request.Expense ? (Math.Abs(request.Amount) * (-1)) : request.Amount;

            c.Name = request.Name;
            c.Amount = request.Expense ? request.ExpenseAmount : request.Amount;
            c.Expense = request.Expense;
            c.Updated = request.Updated;
            c.UpdatedBy = request.UpdatedBy;
            c.EventDate = request.EventDate;

            return _legalManager.UpdateCashMovement(c);
        }

        public class Request
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Amount { get; set; }
            public bool Expense { get; set; }
            public DateTime Updated { get; set; }
            public DateTime EventDate { get; set; } = DateTime.Now;
            public string UpdatedBy { get; set; }
            public double ExpenseAmount => (Math.Abs(Amount) * (-1));
        }
    }
}
