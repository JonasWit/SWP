﻿using SWP.UI.Components.ViewModels.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class OnUpdateCaseRowAction : IAction
    {
        public const string OnUpdateCaseRow = "ON_UPDATE_CASE_ROW";
        public string Name => OnUpdateCaseRow;

        public CaseViewModel Arg { get; set; }
    }
}
