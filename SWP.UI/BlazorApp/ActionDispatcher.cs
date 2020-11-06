using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp
{
    public class ActionDispatcher : IActionDispatcher
    {
        private Action<IAction> _registeredActionHandlers;

        public void Subscribe(Action<IAction> actionHandler) => _registeredActionHandlers += actionHandler;

        public void UnSubscribe(Action<IAction> actionHandler) => _registeredActionHandlers -= actionHandler;

        public void Dispatch(IAction action) => _registeredActionHandlers?.Invoke(action);
    }
}
