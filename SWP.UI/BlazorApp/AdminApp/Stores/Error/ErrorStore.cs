using Microsoft.Extensions.DependencyInjection;
using SWP.UI.BlazorApp.AdminApp.Stores.Application;
using SWP.UI.BlazorApp.AdminApp.Stores.Enums;
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
    public class ErrorStore : StoreBase
    {
        private readonly ErrorState _state;

        public ErrorState GetState() => _state;

        public ErrorStore(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _state = new ErrorState();
        }

        public void SetException(Exception ex) => _state.Exception = ex;

    }
}
