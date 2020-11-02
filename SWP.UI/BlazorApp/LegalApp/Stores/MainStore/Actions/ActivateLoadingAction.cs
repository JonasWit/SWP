using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.MainStore.Actions
{
    public class ActivateLoadingAction : IAction
    {
        public const string ActivateLoading = " ActivateLoading";
        public string Name => ActivateLoading;
        public string LoadingMessage { get; set; }
    }
}
