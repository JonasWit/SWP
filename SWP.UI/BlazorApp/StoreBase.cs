using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp
{
    public abstract class StoreBase
    {
        protected readonly IActionDispatcher _actionDispatcher;
        protected readonly IServiceProvider _serviceProvider;

        public StoreBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #region Observer pattern

        protected Action _listeners;

        public void AddStateChangeListener(Action listener)
        {
            _listeners += listener;
        }

        public void RemoveStateChangeListener(Action listener)
        {
            _listeners -= listener;
        }

        protected void BroadcastStateChange()
        {
            _listeners.Invoke();
        }

        #endregion
    }
}
