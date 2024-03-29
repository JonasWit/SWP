﻿using SWP.UI.Components.ViewModels.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Finance.Actions
{
    public class EditCashMovementRowAction : IAction
    {
        public const string EditCashMovementRow = "EDIT_CASH_MOVEMENT_ROW";
        public string Name => EditCashMovementRow;

        public CashMovementViewModel Arg { get; set; }
    }
}
