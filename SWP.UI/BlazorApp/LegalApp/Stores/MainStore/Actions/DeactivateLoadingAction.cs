using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.MainStore.Actions
{
    public class DeactivateLoadingAction : IAction
    {
        public const string DeactivateLoading = "DeactivateLoading";
        public string Name => DeactivateLoading;
    }
}
