using Microsoft.Extensions.Logging;
using SWP.UI.Components.PortalBlazorComponents.Requests.ViewModels;
using System;
using System.Collections.Generic;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanel
{
    public class RequestsMainPanelState
    {
        public InnerComponents CurrentComponent { get; set; } = InnerComponents.Info;
        public string UserId { get; set; }
        public List<RequestViewModel> Requests { get; set; } = new List<RequestViewModel>();


        public class PresetTitle
        {
            public int MyProperty { get; set; }

        }

        public enum InnerComponents
        { 
            Info = 0,
            Create = 1,
            Details = 2   
        }
    }

    [UIScopedService]
    public class RequestsMainPanelStore : StoreBase<RequestsMainPanelState>
    {
        private readonly ILogger<RequestsMainPanelState> _logger;

        public RequestsMainPanelStore(
            IServiceProvider serviceProvider,
            IActionDispatcher actionDispatcher,
            ILogger<RequestsMainPanelState> logger)
            : base(serviceProvider, actionDispatcher)
        {
            _logger = logger;
        }

        public void Initialize(string userId) => _state.UserId = userId;

        protected override void HandleActions(IAction action)
        {
            switch (action)
            {
                default:
                    break;
            }
        }

        public void SetActiveComponent(RequestsMainPanelState.InnerComponents component)
        {
            _state.CurrentComponent = component;
            BroadcastStateChange();    
        }

        public override void CleanUpStore()
        {

        }

        public override void RefreshSore()
        {

        }
    }
}
