using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.BlazorApp.LegalApp.Stores.MyApp;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents
{
    public partial class LegalSwpMyApp : IDisposable
    {
        [Inject]
        public MainStore MainStore { get; set; }
        [Inject]
        public MyAppStore MyAppStore { get; set; }
        [Inject]
        public TooltipService TooltipService { get; set; }

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(UpdateView);
            MyAppStore.RemoveStateChangeListener(UpdateView);
        }

        private void UpdateView()
        {
            MyAppStore.RefreshData();
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            MainStore.AddStateChangeListener(UpdateView);
            MyAppStore.AddStateChangeListener(UpdateView);
            await MyAppStore.Initialize();
        }

        private string FormatAsPLN(object value) => $"{((double)value).ToString(CultureInfo.CreateSpecificCulture("pl"))} zł";

        private string FormatAsTime(object value)
        {
            var stringValue = value.ToString();

            if (stringValue.Contains('.') ||
                stringValue.Contains(','))
            {
                if (value.ToString().Split('.', ',')[1].Length == 1)
                {
                    return $"{value.ToString().Split('.', ',')[0]}:{value.ToString().Split('.', ',')[1]}0";
                }
                else
                {
                    return $"{value.ToString().Split('.', ',')[0]}:{value.ToString().Split('.', ',')[1]}";
                }
            }
            else
            {
                return stringValue;
            }
        }

        public string RelatedUsersFilterValue = "";

    }
}
