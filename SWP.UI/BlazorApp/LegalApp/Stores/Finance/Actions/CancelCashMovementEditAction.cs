﻿using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Finance.Actions
{
    public class CancelCashMovementEditAction : IAction
    {
        public const string CancelCashMovementEdit = "CANCEL_CASH_MOVEMENT_EDIT";
        public string Name => CancelCashMovementEdit;

        public CashMovementViewModel Arg { get; set; }
    }
}
