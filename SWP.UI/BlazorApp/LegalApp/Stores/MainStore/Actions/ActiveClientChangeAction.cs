using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.MainStore.Actions
{
    public class ActiveClientChangeAction : IAction
    {
        public const string ActiveClientChange = " ActiveClientChange";
        public string Name => ActiveClientChange;
    }
}
