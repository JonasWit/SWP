using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.Application.LegalSwp.Cases;
using SWP.Application.LegalSwp.ContactPeopleAdmin;
using SWP.Application.LegalSwp.Notes;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.LegalApp.Stores.Cases;
using SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions;
using SWP.UI.BlazorApp.LegalApp.Stores.Enums;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpCases
    {
        [Inject]
        public MainStore MainStore { get; set; }
        [Inject]
        public CasesStore CasesStore { get; set; }
        [Inject]
        public GeneralViewModel Gvm { get; set; }
        [Inject]
        public TooltipService TooltipService { get; set; }
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(RefreshView);
            CasesStore.RemoveStateChangeListener(UpdateView);
            CasesStore.CleanUpStore();
        }

        private void UpdateView() => StateHasChanged();

        private void RefreshView()
        {
            if (MainStore.GetState().ActiveClient == null)
            {
                MainStore.SetActivePanel(LegalAppPanels.MyApp);
                return;
            }

            CasesStore.CleanUpStore();
            CasesStore.RefreshSore();
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            CasesStore.EnableLoading(CasesStore.DataLoadingMessage);

            MainStore.AddStateChangeListener(RefreshView);
            CasesStore.AddStateChangeListener(UpdateView);
            CasesStore.Initialize();

            CasesStore.DisableLoading();
        }

        public bool showFirstSection = false;
        public void ShowHideFirstSection() => showFirstSection = !showFirstSection;

        public bool showSecondSection = false;
        public void ShowHideSecondSection() => showSecondSection = !showSecondSection;

        public bool infoBoxVisibleI = false;
        public void ShowHideInfoBoxI() => infoBoxVisibleI = !infoBoxVisibleI;

        public bool infoBoxVisibleII = false;
        public void ShowHideInfoBoxII() => infoBoxVisibleII = !infoBoxVisibleII;

        public bool infoBoxVisibleIII = false;
        public void ShowHideInfoBoxIII() => infoBoxVisibleIII = !infoBoxVisibleIII;

        private void ShowTooltip(ElementReference elementReference, TooltipOptions options = null) => TooltipService.Open(elementReference, options.Text, options);

        #region Actions 

        private void CreateNewCase(CreateCase.Request request) => ActionDispatcher.Dispatch(new CreateNewCaseAction { Request = request });

        private void EditCaseRow(CaseViewModel data) => ActionDispatcher.Dispatch(new EditCaseRowAction { Arg = data });

        private void OnUpdateCaseRow(CaseViewModel data) => ActionDispatcher.Dispatch(new OnUpdateCaseRowAction { Arg = data });

        private void CancelEditCaseRow(CaseViewModel data) => ActionDispatcher.Dispatch(new CancelEditCaseRowAction { Arg = data });

        private void SaveCaseRow(CaseViewModel data) => ActionDispatcher.Dispatch(new SaveCaseRowAction { Arg = data });

        private void DeleteCaseRow(CaseViewModel data) => ActionDispatcher.Dispatch(new DeleteCaseRowAction { Arg = data });

        private void ActiveCaseChange(object data) => ActionDispatcher.Dispatch(new ActiveCaseChangeAction { Arg = data });

        private void ArchivizeCase(CaseViewModel data) => ActionDispatcher.Dispatch(new ArchivizeCaseAction { Arg = data });

        private void ActiveNoteChange(object data) => ActionDispatcher.Dispatch(new ActiveNoteChangeAction { Arg = data });

        private void CreateNewNote(CreateNote.Request request) => ActionDispatcher.Dispatch(new CreateNewNoteAction { Request = request });

        private void EditNoteRow(NoteViewModel request) => ActionDispatcher.Dispatch(new EditNoteRowAction { Arg = request });

        private void OnUpdateNoteRow(NoteViewModel request) => ActionDispatcher.Dispatch(new OnUpdateNoteRowAction { Arg = request });

        private void SaveNoteRow(NoteViewModel request) => ActionDispatcher.Dispatch(new SaveNoteRowAction { Arg = request });

        private void CancelEditNoteRow(NoteViewModel request) => ActionDispatcher.Dispatch(new CancelEditNoteRowAction { Arg = request });

        private void DeleteNoteRow(NoteViewModel request) => ActionDispatcher.Dispatch(new DeleteNoteRowAction { Arg = request });

        private void OnSlotSelect(SchedulerSlotSelectEventArgs request) => ActionDispatcher.Dispatch(new OnSlotSelectAction { Arg = request });

        private void OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<ReminderViewModel> args) => ActionDispatcher.Dispatch(new OnAppointmentSelectAction { Arg = args });

        private void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<ReminderViewModel> args) => ActionDispatcher.Dispatch(new OnAppointmentRenderAction { Arg = args });

        private void EditContactRow(ContactPersonViewModel args) => ActionDispatcher.Dispatch(new EditContactRowAction { Arg = args });

        private void OnUpdateContactRow(ContactPersonViewModel args) => ActionDispatcher.Dispatch(new OnUpdateContactRowAction { Arg = args });

        private void SaveContactRow(ContactPersonViewModel args) => ActionDispatcher.Dispatch(new SaveContactRowAction { Arg = args });

        private void CancelContactEdit(ContactPersonViewModel contact) => ActionDispatcher.Dispatch(new CancelContactEditAction { Arg = contact });

        private void DeleteContactRow(ContactPersonViewModel contact) => ActionDispatcher.Dispatch(new DeleteContactRowAction { Arg = contact });

        private void SubmitNewContact(CreateContactPerson.Request request) => ActionDispatcher.Dispatch(new SubmitNewContactAction { Request = request });

        private void ContactSelected(object arg) => ActionDispatcher.Dispatch(new ContactSelectedAction { Arg = arg });

        #endregion

    }
}
