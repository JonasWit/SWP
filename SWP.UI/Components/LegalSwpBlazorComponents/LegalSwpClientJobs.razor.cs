using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.Application.LegalSwp.Jobs;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.LegalApp.Stores.ClientJobs;
using SWP.UI.BlazorApp.LegalApp.Stores.ClientJobs.Actions;
using SWP.UI.BlazorApp.LegalApp.Stores.Enums;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpClientJobs
    {
        [Inject]
        public GeneralViewModel Gvm { get; set; }
        [Inject]
        public TooltipService TooltipService { get; set; }
        [Inject]
        public MainStore MainStore { get; set; }
        [Inject]
        public ClientJobsStore ClientJobsStore { get; set; }
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public string ArchvizedClientsFilterValue;

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(RefreshView);
            ClientJobsStore.RemoveStateChangeListener(UpdateView);
            ClientJobsStore.CleanUpStore();
        }

        private void UpdateView()
        {
            StateHasChanged();
        }

        private void RefreshView()
        {
            if (MainStore.GetState().ActiveClient == null)
            {
                MainStore.SetActivePanel(LegalAppPanels.MyApp);
                return;
            }

            ClientJobsStore.CleanUpStore();
            ClientJobsStore.RefreshSore();
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            MainStore.AddStateChangeListener(RefreshView);
            ClientJobsStore.AddStateChangeListener(UpdateView);
            ClientJobsStore.Initialize();
        }

        private void ShowTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, options.Text, options);

        public string ArchvizedJobsFilterValue;

        public bool showFormVisible = false;
        public void ShowHideForm() => showFormVisible = !showFormVisible;

        public bool addClientformVisible = false;
        public void ShowHideClientFormI() => addClientformVisible = !addClientformVisible;

        public bool showSecondSection = false;
        public void ShowHideSecondSection() => showSecondSection = !showSecondSection;

        public bool clientListInfoVisible = false;
        public void ShowHideClientI() => clientListInfoVisible = !clientListInfoVisible;

        public bool InfoVisible = false;
        public void ShowHideInfo() => InfoVisible = !InfoVisible;


        #region Actions

        private void SubmitNewClientJob(CreateClientJob.Request arg) => ActionDispatcher.Dispatch(new SubmitNewClientJobAction { Arg = arg });

        private void EditClientJobRow(ClientJobViewModel arg) => ActionDispatcher.Dispatch(new EditClientJobRowAction { Arg = arg });

        private void OnUpdateClientJobRow(ClientJobViewModel arg) => ActionDispatcher.Dispatch(new OnUpdateClientJobRowAction { Arg = arg });

        private void SaveClientJobRow(ClientJobViewModel arg) => ActionDispatcher.Dispatch(new SaveClientJobRowAction { Arg = arg });

        private void CancelClientJobEdit(ClientJobViewModel arg) => ActionDispatcher.Dispatch(new CancelClientJobEditAction { Arg = arg });

        private void DeleteClientJobRow(ClientJobViewModel arg) => ActionDispatcher.Dispatch(new DeleteClientJobRowAction { Arg = arg });

        private void ActiveJobChange(object arg) => ActionDispatcher.Dispatch(new ActiveJobChangeAction { Arg = arg });

        private void ArchivizeClientJob(ClientJobViewModel arg) => ActionDispatcher.Dispatch(new ArchivizeClientJobAction { Arg = arg });
        
        private void SelectedArchivizedClientJobChange(object arg) => ActionDispatcher.Dispatch(new SelectedArchivizedClientJobChangeAction { Arg = arg });

        private void RecoverSelectedJob() => ActionDispatcher.Dispatch(new RecoverSelectedJobAction());


        #endregion

    }
}
