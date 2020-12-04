using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp
{
    public abstract class StoreBase<TState> where TState : class, new()
    {
        public string DataLoadingMessage => @"Wczytywanie danych...";

        protected readonly IActionDispatcher _actionDispatcher;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly NotificationService _notificationService;
        protected readonly DialogService _dialogService;

        protected readonly TState _state;
        public TState GetState() => _state;

        public bool Loading { get; set; } = false;
        public string LoadingMessage { get; set; } = "";

        public void EnableLoading(string message)
        {
            LoadingMessage = message;
            Loading = true;
        }

        public void DisableLoading()
        {
            Loading = false;
        }
 
        public void ShowNotification(NotificationSeverity severity, string summary, string detail, int duration) =>
            _notificationService.Notify(new NotificationMessage() { Severity = severity, Summary = summary, Detail = detail, Duration = duration });

        public StoreBase(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, DialogService dialogService)
        {
            _serviceProvider = serviceProvider;
            _actionDispatcher = actionDispatcher;
            _notificationService = notificationService;
            _dialogService = dialogService;
            _actionDispatcher.Subscribe(HandleActions);
            _state = new TState();
        }

        public StoreBase(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService)
        {
            _serviceProvider = serviceProvider;
            _actionDispatcher = actionDispatcher;
            _notificationService = notificationService;
            _actionDispatcher.Subscribe(HandleActions);
            _state = new TState();
        }

        public StoreBase(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher)
        {
            _serviceProvider = serviceProvider;
            _actionDispatcher = actionDispatcher;
            _actionDispatcher.Subscribe(HandleActions);
            _state = new TState();
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
            _listeners?.Invoke();
        }

        #endregion

        public abstract void CleanUpStore();

        public virtual void RefreshSore()
        {
            ShowNotification(NotificationSeverity.Success, "Sukces!", "Panel Odświeżony", 1500);
        }
    }
}
