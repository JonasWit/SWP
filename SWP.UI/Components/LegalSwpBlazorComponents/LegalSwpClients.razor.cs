using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.Application.LegalSwp.Clients;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.LegalApp.Stores.Clients;
using SWP.UI.BlazorApp.LegalApp.Stores.Clients.Actions;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpClients : IDisposable
    {
        [Inject]
        public MainStore MainStore { get; set; }
        [Inject]
        public ClientsStore ClientsStore { get; set; }
        [Inject]
        public TooltipService TooltipService { get; set; }
        [Inject]
        public GeneralViewModel Gvm { get; set; }
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(UpdateView);
            ClientsStore.RemoveStateChangeListener(UpdateView);
            ClientsStore.CleanUpStore();
        }

        private void UpdateView()
        {
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            MainStore.AddStateChangeListener(UpdateView);
            ClientsStore.AddStateChangeListener(UpdateView);
        }

        private void ShowTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, options.Text, options);

        public bool addClientformVisible = false;
        public void ShowHideClientFormI() => addClientformVisible = !addClientformVisible;

        public bool clientListInfoVisible = false;
        public void ShowHideClientI() => clientListInfoVisible = !clientListInfoVisible;

        public bool showFormVisible = false;
        public void ShowHideForm() => showFormVisible = !showFormVisible;

        public bool showClientListVisible = false;
        public void ShowHideClientsList() => showClientListVisible = !showClientListVisible;

        #region Actions

        private void EditClientRow(ClientViewModel arg) => ActionDispatcher.Dispatch(new EditClientRowAction { Arg = arg });

        private void OnUpdateClientRow(ClientViewModel arg) => ActionDispatcher.Dispatch(new OnUpdateClientRowAction { Arg = arg });

        private void SaveClientRow(ClientViewModel arg) => ActionDispatcher.Dispatch(new SaveClientRowAction { Arg = arg });

        private void CancelClientEdit(ClientViewModel arg) => ActionDispatcher.Dispatch(new CancelClientEditAction { Arg = arg });

        private void DeleteClientRow(ClientViewModel arg) => ActionDispatcher.Dispatch(new DeleteClientRowAction { Arg = arg });

        private void ArchivizeClient(ClientViewModel arg) => ActionDispatcher.Dispatch(new ArchivizeClientAction { Arg = arg });

        private void SubmitNewClient(CreateClient.Request arg) => ActionDispatcher.Dispatch(new SubmitNewClientAction { Arg = arg });

        #endregion
    }
}
