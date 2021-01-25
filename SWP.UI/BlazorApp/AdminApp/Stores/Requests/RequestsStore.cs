using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Radzen;
using Radzen.Blazor;
using SWP.Application.PortalCustomers.RequestsManagement;
using SWP.Domain.Enums;
using SWP.Domain.Models.Portal.Communication;
using SWP.UI.BlazorApp.AdminApp.Stores.Application;
using SWP.UI.BlazorApp.AdminApp.Stores.Requests.Actions;
using SWP.UI.BlazorApp.AdminApp.Stores.StatusLog;
using SWP.UI.Components.PortalBlazorComponents.Requests.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Communication
{
    public class RequestsState
    {
        public string Body { get; set; }
        public string Title { get; set; }

        public List<Recipient> Recipients { get; set; } = new List<Recipient>();
        public IEnumerable<string> SelectedRecipients { get; set; }
        public IEnumerable<int> UserTypes { get; set; }
        public IEnumerable<int> ApplicationTypes { get; set; }

        public RequestViewModel SelectedRequest { get; set; }
        public List<RequestViewModel> Requests { get; set; } = new List<RequestViewModel>();
        public RadzenGrid<RequestViewModel> RequestsGrid { get; set; }

        public RequestMessageViewModel SelectedRequestMessage { get; set; }
        public CreateRequest.RequestMessage NewRequestMessage { get; set; } = new CreateRequest.RequestMessage();

        public List<RequestStatus> RequestStatuses { get; set; } = new List<RequestStatus>();
        public int SelectedRequestStatus { get; set; }

        public class RequestStatus
        {
            public int Id { get; set; }
            public string Status { get; set; }
        }
    }

    [UIScopedService]
    public class RequestsStore : StoreBase<RequestsState>
    {
        private readonly ILogger<CommunicationStore> _logger;

        StatusBarStore StatusBarStore => _serviceProvider.GetRequiredService<StatusBarStore>();
        ApplicationStore MainStore => _serviceProvider.GetRequiredService<ApplicationStore>();

        public RequestsStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, ILogger<CommunicationStore> logger)
            : base(serviceProvider, actionDispatcher, notificationService)
        {
            _logger = logger;
            _state.RequestStatuses = new List<RequestsState.RequestStatus>();

            foreach (RequestStatus en in Enum.GetValues(typeof(RequestStatus)))
            {
                _state.RequestStatuses.Add(new RequestsState.RequestStatus {Id = (int)en, Status = en.ToString() });
            }
        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case AdminRequestSelectedChangeAction.AdminCommRequestSelectedChange:
                    var adminRequestSelectedChangeAction = (AdminRequestSelectedChangeAction)action;
                    AdminRequestSelectedChange(adminRequestSelectedChangeAction.Arg);
                    break;
                case AdminRequestMessageSelectedChangeAction.AdminRequestMessageSelectedChange:
                    var adminRequestMessageSelectedChangeAction = (AdminRequestMessageSelectedChangeAction)action;
                    AdminRequestMessageSelectedChange(adminRequestMessageSelectedChangeAction.Arg);
                    break;
                case AdminSubmitNewRequestResponseAction.AdminSubmitNewRequestResponse:
                    var adminSubmitNewRequestResponseAction = (AdminSubmitNewRequestResponseAction)action;
                    await CreateNewRequestMessage(adminSubmitNewRequestResponseAction.Arg);
                    break;
                case AdminUpdateRequestStatusAction.AdminUpdateRequestStatus:
                    await UpdateRequestStatus();
                    break;
                default:
                    break;
            }
        }

        #region Communication

        private async Task CreateNewRequestMessage(CreateRequest.RequestMessage arg)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createRequest = scope.ServiceProvider.GetRequiredService<CreateRequest>();

                await createRequest.Create(new CreateRequest.RequestMessage
                {
                    AuthorName = MainStore.GetState().AppActiveUserManager.UserName,
                    Message = arg.Message
                }, _state.SelectedRequest.Id);

                _state.SelectedRequest = GetRequest(_state.SelectedRequest.Id);
                ShowNotification(NotificationSeverity.Success, "Done!", $"New Message Added!", 5000);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        private async Task UpdateRequestStatus()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var updateRequest = scope.ServiceProvider.GetRequiredService<UpdateRequest>();

                var request = _state.SelectedRequest;
                request.Status = (RequestStatus)_state.SelectedRequestStatus;
                request.Updated = DateTime.Now;
                request.UpdatedBy = MainStore.GetState().AppActiveUserManager.UserName;

                var result = await updateRequest.Update(request);

                GetRequests();
                await _state.RequestsGrid.Reload();

                _state.SelectedRequest = GetRequest(request.Id);
                _state.SelectedRequestStatus = (int)_state.SelectedRequest.Status;

                ShowNotification(NotificationSeverity.Success, "Done!", $"Status changed to {_state.SelectedRequest.Status}!", 5000);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                MainStore.ShowErrorPage(ex);
            }
        }

        public void GetRequests()
        {
            using var scope = _serviceProvider.CreateScope();
            var getRequest = scope.ServiceProvider.GetRequiredService<GetRequest>();

            _state.Requests = getRequest.GetRequests().Select(x => (RequestViewModel)x).ToList().OrderByDescending(x => x.Updated).ToList();
        }

        private void AdminRequestSelectedChange(RequestViewModel arg)
        {
            _state.SelectedRequest = GetRequest(arg.Id);
            _state.SelectedRequestStatus = (int)_state.SelectedRequest.Status;
            BroadcastStateChange();
        }

        private void AdminRequestMessageSelectedChange(int arg)
        {
            _state.SelectedRequestMessage = _state.SelectedRequest.Messages.FirstOrDefault(x => x.Id.Equals(arg));
            BroadcastStateChange();
        }

        private ClientRequest GetRequest(int requestId)
        {
            using var scope = _serviceProvider.CreateScope();
            var getRequest = scope.ServiceProvider.GetRequiredService<GetRequest>();

            var result = getRequest.GetRequestWithMessages(requestId);
            result.Messages = result.Messages.OrderByDescending(x => x.Created).ToList();

            return result;
        }

        #endregion
    }
}
