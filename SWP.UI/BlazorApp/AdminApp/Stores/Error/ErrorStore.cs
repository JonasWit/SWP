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
        public ErrorStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher) 
            : base(serviceProvider, actionDispatcher)
        {

        }

        public Task SetException(Exception ex, string userId)
        {
            _state.Exception = ex;
            //todo:add logging!

            //return _portalLogger.CreateLogRecord(new CreateLogRecord.Request
            //{
            //    Message = ex.Message,
            //    UserId = userId,
            //    StackTrace = ex.StackTrace
            //});

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
