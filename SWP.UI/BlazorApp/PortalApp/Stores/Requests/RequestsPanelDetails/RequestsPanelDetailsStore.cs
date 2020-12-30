using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SWP.Application.PortalCustomers.RequestsManagement;
using SWP.Domain.Models.Portal.Communication;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanel;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanelDetails.Actions;
using SWP.UI.Components.PortalBlazorComponents.Requests.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests
{
    public class RequestsPanelDetailsState
    {
        public RequestViewModel ActiveRequest { get; set; } = new RequestViewModel();
        public RequestMessageViewModel ActiveRequestMessage { get; set; }
        public CreateRequest.RequestMessage NewRequestMessage { get; set; } = new CreateRequest.RequestMessage();
    }

    [UIScopedService]
    public class RequestsPanelDetailsStore : StoreBase<RequestsPanelDetailsState>
    {
        private readonly ILogger<RequestsPanelDetailsStore> _logger;
        public RequestsMainPanelStore MainStore => _serviceProvider.GetRequiredService<RequestsMainPanelStore>();

        public RequestsPanelDetailsStore(
            IServiceProvider serviceProvider,
            IActionDispatcher actionDispatcher,
            ILogger<RequestsPanelDetailsStore> logger)
            : base(serviceProvider, actionDispatcher)
        {
            _logger = logger;
        }

        public void Initialize()
        {
            EnableLoading("Wczytywanie historii...");

            _state.ActiveRequest = GetRequest(MainStore.GetState().SelectedRequestId);

            DisableLoading();
        }

        private ClientRequest GetRequest(int requestId)
        {
            using var scope = _serviceProvider.CreateScope();
            var getRequest = scope.ServiceProvider.GetRequiredService<GetRequest>();

            var result = getRequest.GetRequestWithMessages(requestId);
            result.Messages = result.Messages.OrderByDescending(x => x.Created).ToList();

            return result;
        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case RequestMessageSelectedAction.RequestMessageSelected:
                    var requestMessageSelectedAction = (RequestMessageSelectedAction)action;
                    ShowRequestMessageDetails(requestMessageSelectedAction.Arg);
                    break;
                case CreateNewRequestMessageAction.CreateNewRequestMessage:
                    var createNewRequestMessageAction = (CreateNewRequestMessageAction)action;
                    await CreateNewRequestMessage(createNewRequestMessageAction.Arg);
                    break;
                default:
                    break;
            }
        }

        private async Task CreateNewRequestMessage(CreateRequest.RequestMessage request)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createRequest = scope.ServiceProvider.GetRequiredService<CreateRequest>();

                await createRequest.Create(new CreateRequest.RequestMessage
                {
                    AuthorId = MainStore.GetState().ActiveUserName,
                    Message = request.Message
                }, _state.ActiveRequest.Id);

                MainStore.RefreshSore();
            }
            catch (Exception ex)
            {
                MainStore.HandleError(ex, "Exception in Portal Contact App during Message creation.");
            }
        }

        private void ShowRequestMessageDetails(int id)
        {
            _state.ActiveRequestMessage = _state.ActiveRequest.Messages.FirstOrDefault(x => x.Id == id);
            BroadcastStateChange();
        }

        public override void CleanUpStore()
        {

        }

        public override void RefreshSore()
        {
            _state.ActiveRequest = GetRequest(MainStore.GetState().SelectedRequestId);
        }
    }
}
