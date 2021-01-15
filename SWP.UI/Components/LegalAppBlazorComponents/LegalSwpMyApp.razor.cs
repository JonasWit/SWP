using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.BlazorApp.LegalApp.Stores.MyApp;
using SWP.UI.Utilities;
using System;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalAppBlazorComponents
{
    public partial class LegalSwpMyApp : IDisposable
    {
        [Inject]
        public MainStore MainStore { get; set; }
        [Inject]
        public MyAppStore Store { get; set; }
        [Inject]
        public TooltipService TooltipService { get; set; }
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(RefreshView);
            Store.RemoveStateChangeListener(RefreshView);
        }

        private void RefreshView()
        {
            Store.RefreshSore();
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            Store.EnableLoading(Store.DataLoadingMessage);

            MainStore.AddStateChangeListener(RefreshView);
            Store.AddStateChangeListener(RefreshView);
            await Store.Initialize();

            Store.DisableLoading();
        }

        private static string FormatAsPLN(object value)
        {
            var amount = Convert.ToDouble(value);
            return amount.FormatPLN();
        }

        private static string FormatAsTime(object value)
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
