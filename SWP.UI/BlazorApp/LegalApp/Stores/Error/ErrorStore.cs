using System;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Error
{
    public class ErrorState
    {
        public bool Loading { get; set; } = false;
        public string LoadingMessage { get; set; }
        public Exception Exception { get; set; }
    }

    [UIScopedService]
    public class ErrorStore : StoreBase<ErrorState>
    {
        public ErrorStore(
            IServiceProvider serviceProvider, 
            IActionDispatcher actionDispatcher) 
            : base(serviceProvider, actionDispatcher)
        {

        }

        public Task SetException(Exception ex, string userId)
        {
            _state.Exception = ex;
            //return _portalLogger.LogException(ex, userId);
            return Task.CompletedTask;
        }

        protected override void HandleActions(IAction action)
        {

        }

        public override void CleanUpStore()
        {

        }

        public override void RefreshSore()
        {

        }
    }
}
