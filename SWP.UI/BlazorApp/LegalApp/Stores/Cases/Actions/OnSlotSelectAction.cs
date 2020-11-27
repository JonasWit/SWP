using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class OnSlotSelectAction : IAction
    {
        public const string OnSlotSelect = "ON_SLOT_SELECT";
        public string Name => OnSlotSelect;

        public SchedulerSlotSelectEventArgs Arg { get; set; }
    }
}
