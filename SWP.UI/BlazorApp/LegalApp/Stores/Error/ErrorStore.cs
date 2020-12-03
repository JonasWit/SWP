using SWP.Application.Log;
using SWP.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly PortalLogger _portalLogger;
        private readonly CreateLogRecord _createLogRecord;

        public ErrorStore(
            IServiceProvider serviceProvider, 
            IActionDispatcher actionDispatcher, 
            CreateLogRecord createLogRecord,
            PortalLogger portalLogger) 
            : base(serviceProvider, actionDispatcher)
        {
            _portalLogger = portalLogger;
            _createLogRecord = createLogRecord;
        }

        public Task SetException(Exception ex, string userId)
        {
            _state.Exception = ex;
            return _portalLogger.LogException(ex, userId);
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
