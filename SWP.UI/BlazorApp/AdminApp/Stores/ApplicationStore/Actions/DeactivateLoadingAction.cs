using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.ApplicationStore.Actions
{
    public class DeactivateLoadingAction : IAction
    {
        public const string DeactivateLoading = "DeactivateLoading";
        public string Name => DeactivateLoading;
    }
}
