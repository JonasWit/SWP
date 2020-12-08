using Microsoft.Extensions.Logging;
using SWP.UI.Utilities;
using System;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Error
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
        private readonly ILogger<ErrorStore> _logger;

        public ErrorStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, ILogger<ErrorStore> logger) 
            : base(serviceProvider, actionDispatcher)
        {
            _logger = logger;
        }

        public void SetException(Exception ex, string userId, string userName)
        {
            _state.Exception = ex;
            _logger.LogError(ex, LogTags.AdminAppErrorLogPrefix + "Exception for user {userId} - {userName}", userId, userName);
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
