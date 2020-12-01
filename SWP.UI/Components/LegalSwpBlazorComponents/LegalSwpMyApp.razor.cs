using Microsoft.AspNetCore.Components;
using Radzen;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.BlazorApp.LegalApp.Stores.MyApp;
using SWP.UI.BlazorApp.LegalApp.Stores.MyApp.Actions;
using SWP.UI.Utilities;
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
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        public void Dispose()
        {
            MainStore.RemoveStateChangeListener(RefreshView);
            MyAppStore.RemoveStateChangeListener(RefreshView);
            MyAppStore.CleanUpStore();
        }

        private void RefreshView()
        {
            MyAppStore.RefreshSore();
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            MyAppStore.EnableLoading(MyAppStore.DataLoadingMessage);

            MainStore.AddStateChangeListener(RefreshView);
            MyAppStore.AddStateChangeListener(RefreshView);
            await MyAppStore.Initialize();

            MyAppStore.DisableLoading();
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

        #region Actions

        private void SelectedUserChange(object arg) => ActionDispatcher.Dispatch(new SelectedUserChangeAction { Arg = arg });










        #endregion

    }
}
