using Microsoft.AspNetCore.Components;
using Radzen;
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
        protected readonly NotificationService _notificationService;
        protected readonly DialogService _dialogService;

        public void ShowNotification(NotificationSeverity severity, string summary, string detail, int duration) =>
            _notificationService.Notify(new NotificationMessage() { Severity = severity, Summary = summary, Detail = detail, Duration = duration });

        public StoreBase(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, DialogService dialogService)
        {
            _serviceProvider = serviceProvider;
            _actionDispatcher = actionDispatcher;
            _notificationService = notificationService;
            _dialogService = dialogService;
            _actionDispatcher.Subscribe(HandleActions);
        }

        public StoreBase(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService)
        {
            _serviceProvider = serviceProvider;
            _actionDispatcher = actionDispatcher;
            _notificationService = notificationService;
            _actionDispatcher.Subscribe(HandleActions);
        }

        public StoreBase(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher)
        {
            _serviceProvider = serviceProvider;
            _actionDispatcher = actionDispatcher;
            _actionDispatcher.Subscribe(HandleActions);
        }

        protected abstract void HandleActions(IAction action);

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
