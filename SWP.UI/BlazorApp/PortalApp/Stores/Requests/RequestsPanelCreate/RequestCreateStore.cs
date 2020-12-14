using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SWP.Application.PortalCustomers.LicenseManagement;
using SWP.Application.PortalCustomers.RequestsManagement;
using SWP.Domain.Enums;
using SWP.Domain.Models.Portal;
using SWP.UI.BlazorApp.PortalApp.Stores.Requests.RequestsPanelCreate.Actions;
using SWP.UI.Components.PortalBlazorComponents.Requests.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
            public int ChosenRequestReason { get; set; } = 0;
            public int ChosenApplicationReason { get; set; } = 0;
            public RequestReason NewRequestReason { get; set; } = RequestReason.Query;
            public ApplicationType NewRequestApplication { get; set; } = ApplicationType.NoApp;
            public string Message { get; set; }
        }
    }

    [UIScopedService]
    public class RequestCreateStore : StoreBase<RequestCreateState>
    {
        private readonly ILogger<RequestCreateStore> _logger;

        public RequestCreateStore(
            IServiceProvider serviceProvider,
            IActionDispatcher actionDispatcher,
            ILogger<RequestCreateStore> logger)
            : base(serviceProvider, actionDispatcher)
        {
            _logger = logger;
        }

        public async Task Initialize(string userId)
        {
            using var scope = _serviceProvider.CreateScope();
            var getLicense = scope.ServiceProvider.GetRequiredService<GetLicense>();

            _state.UserLicenses = getLicense.GetAll(userId);
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

            _state.StepsConfig.ChosenApplicationReason = (int)arg;
            _state.StepsConfig.NewRequestApplication = (ApplicationType)(int)arg;
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

        private async Task CreateNewRequest(CreateRequest.Request request)
        {
            //try
            //{
            //    using var scope = _serviceProvider.CreateScope();
            //    var createClientJob = scope.ServiceProvider.GetRequiredService<CreateClientJob>();

            //    request.ClientId = MainStore.GetState().ActiveClient.Id;
            //    request.UpdatedBy = MainStore.GetState().User.UserName;

            //    var result = await createClientJob.Create(request);
            //    _state.NewClientJob = new CreateClientJob.Request();

            //    _state.Jobs.Add(result);

            //    await _state.ClientsJobsGrid.Reload();
            //    ShowNotification(NotificationSeverity.Success, "Sukces!", $"Zadanie: {result.Name}, dla Klineta: {MainStore.GetState().ActiveClient.Name} zostało stworzone.", GeneralViewModel.NotificationDuration);
            //    BroadcastStateChange();
            //}
            //catch (Exception ex)
            //{
            //    MainStore.ShowErrorPage(ex);
            //}
        }

        public override void CleanUpStore()
        {

        }

        public override void RefreshSore()
        {

        }
    }
}
