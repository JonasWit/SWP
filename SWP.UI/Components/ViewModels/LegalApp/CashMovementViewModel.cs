﻿using SWP.Domain.Models.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.ViewModels.LegalApp
{
    public class CashMovementViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public int ClientId { get; set; }
        public bool Expense { get; set; }
        public DateTime EventDate { get; set; }

        public static implicit operator CashMovementViewModel(CashMovement input) =>
            new CashMovementViewModel
            {
                Id = input.Id,
                Amount = input.Amount,
                Name = input.Name,
                ClientId = input.ClientId,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy,
                EventDate = input.EventDate,
                Expense = input.Expense
            };
    }
}
