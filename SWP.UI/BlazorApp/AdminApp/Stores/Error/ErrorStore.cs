using Microsoft.Extensions.DependencyInjection;
using SWP.Application.Log;
using SWP.UI.BlazorApp.AdminApp.Stores.Application;
using SWP.UI.BlazorApp.AdminApp.Stores.Enums;
using SWP.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly PortalLogger _portalLogger;

        public ErrorStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, PortalLogger portalLogger) 
            : base(serviceProvider, actionDispatcher)
        {
            _portalLogger = portalLogger;
        }

        public Task SetException(Exception ex, string userId)
        {
            _state.Exception = ex;
            return _portalLogger.CreateLogRecord(new CreateLogRecord.Request
            {
                Message = ex.Message,
                UserId = userId,
                StackTrace = ex.StackTrace
            });
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
