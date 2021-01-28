using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SWP.Application.PortalCustomers.LicenseManagement;
using SWP.Application.PortalCustomers.RequestsManagement;
using SWP.Domain.Enums;
using SWP.Domain.Models.Portal;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanel;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanelCreate.Actions;
using SWP.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanelCreate
{
    public class RequestCreateState
    {
        public StepsConfiguration StepsConfig { get; set; } = new StepsConfiguration();
        public CreateRequest.Request NewRequest { get; set; } = new CreateRequest.Request();
        public List<UserLicense> UserLicenses { get; set; } = new List<UserLicense>();

        public class StepsConfiguration
        {
            public int ChosenRequestReason { get; set; }
            public int ChosenApplication { get; set; }
            public RequestReason NewRequestReason { get; set; }
            public ApplicationType NewRequestApplication { get; set; }
            public string Message { get; set; }

            public StepsConfiguration()
            {
                ChosenApplication = 0;
                ChosenRequestReason = 0;
                NewRequestApplication = (ApplicationType)ChosenApplication;
                NewRequestReason = RequestReason.Query;
            }
        }
    }

    [UIScopedService]
    public class RequestCreateStore : StoreBase<RequestCreateState>
    {
        private readonly ILogger<RequestCreateStore> _logger;
        public RequestsMainPanelStore MainStore => _serviceProvider.GetRequiredService<RequestsMainPanelStore>();

        public RequestCreateStore(
            IServiceProvider serviceProvider,
            IActionDispatcher actionDispatcher,
            ILogger<RequestCreateStore> logger)
            : base(serviceProvider, actionDispatcher)
        {
            _logger = logger;
        }

        public void SetRelatedUsersNumber(int number) => _state.NewRequest.RelatedUsers = number;

        public void Initialize()
        {
            using var scope = _serviceProvider.CreateScope();
            var getLicense = scope.ServiceProvider.GetRequiredService<GetLicense>();

            _state.UserLicenses = getLicense.GetAll(MainStore.GetState().ActiveUserId);
        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case SelectedRequestReasonChangeAction.SelectedRequestSubjectChange:
                    var selectedRequestReasonChangeAction = (SelectedRequestReasonChangeAction)action;
                    SelectedRequestReasonChange(selectedRequestReasonChangeAction.Arg);
                    break;
                case CreateNewRequestAction.CreateNewRequest:
                    var createNewRequestAction = (CreateNewRequestAction)action;
                    await CreateNewRequest(createNewRequestAction.Arg);
                    break;
                case SelectedRequestApplicationChangeAction.SelectedRequestApplicationChange:
                    var selectedRequestApplicationChangeAction = (SelectedRequestApplicationChangeAction)action;
                    SelectedRequestApplicationChange(selectedRequestApplicationChangeAction.Arg);
                    break;
                default:
                    break;
            }
        }

        private void SelectedRequestReasonChange(object arg)
        {
            if (arg is null) return;

            _state.StepsConfig.ChosenRequestReason = (int)arg;
            _state.StepsConfig.NewRequestReason = (RequestReason)(int)arg;
        }

        private void SelectedRequestApplicationChange(object arg)
        {
            if (arg is null) return;

            _state.StepsConfig.ChosenApplication = (int)arg;
            _state.StepsConfig.NewRequestApplication = (ApplicationType)(int)arg;
        }

        private async Task CreateNewRequest(CreateRequest.Request request)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createRequest = scope.ServiceProvider.GetRequiredService<CreateRequest>();
                var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

                await createRequest.Create(new CreateRequest.Request
                {
                    RequestorName = MainStore.GetState().ActiveUserName,
                    Application = (int)_state.StepsConfig.NewRequestApplication,
                    Created = DateTime.Now,
                    CreatedBy = MainStore.GetState().ActiveUserId,
                    Reason = (int)_state.StepsConfig.NewRequestReason,
                    RelatedUsers = request.RelatedUsers,
                    LicenseMonths = request.LicenseMonths,
                    Status = (int)RequestStatus.WaitingForAnswer,
                    RequestMessage = new CreateRequest.RequestMessage
                    {
                        AuthorName = MainStore.GetState().ActiveUserName,
                        Message = request.RequestMessage.Message
                    },
                    AutoRenewal = request.AutoRenewal
                });

                _state.NewRequest = new CreateRequest.Request(); 
  
                await emailSender.SendEmailAsync(PortalNames.InternalEmail.Office, $"---New Request From: {MainStore.GetState().ActiveUserName}---", $"Check Admin Panel, Reason: {(int)_state.StepsConfig.NewRequestReason}");
                await MainStore.SetActiveComponent(RequestsMainPanelState.InnerComponents.Info);
            }
            catch (Exception ex)
            {
                MainStore.HandleError(ex, "Exception in Portal Contact App during Request creation.");
            }
        }
    }
}
