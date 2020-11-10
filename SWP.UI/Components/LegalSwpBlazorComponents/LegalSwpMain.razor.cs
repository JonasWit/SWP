using Microsoft.AspNetCore.Components;
using SWP.UI.Components.LegalSwpBlazorComponents.App;
using System;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpMain : IDisposable
    {
        //[Inject]
        //public MainStore MainStore { get; set; }
        //[Parameter]
        //public string ActiveUserId { get; set; }

        //public void Dispose()
        //{
        //    MainStore.RemoveStateChangeListener(UpdateView);
        //}

        //private void UpdateView() => StateHasChanged();

        //protected override async Task OnInitializedAsync()
        //{
        //    base.OnInitialized();
        //    MainStore.AddStateChangeListener(UpdateView);
        //    await MainStore.InitializeState(ActiveUserId);
        //}


        [Parameter]
        public string ActiveUserId { get; set; }
        [Inject]
        public LegalBlazorApp App { get; set; }

        private bool initializing = false;

        protected override async Task OnInitializedAsync()
        {
            initializing = true;

            await App.Initialize(ActiveUserId);
            App.CallStateHasChanged += new EventHandler(CallStateHasChanged);

            initializing = false;
        }

        public void CallStateHasChanged(object sender, EventArgs e) => StateHasChanged();

        public void Dispose() => App.CallStateHasChanged -= new EventHandler(CallStateHasChanged);

        public bool showFormVisible = false;
        public void ShowHideForm() => showFormVisible = !showFormVisible;

    }
}
