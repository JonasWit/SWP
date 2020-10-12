﻿using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.CashMovements;
using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class FinancePage : BlazorPageBase
    {
        private CreateCashMovement CreateCashMovement => serviceProvider.GetService<CreateCashMovement>();
        private DeleteCashMovement DeleteCashMovement => serviceProvider.GetService<DeleteCashMovement>();
        private UpdateCashMovement UpdateCashMovement => serviceProvider.GetService<UpdateCashMovement>();

        public LegalBlazorApp App { get; private set; }
        public CreateCashMovement.Request NewCashMovement { get; set; } = new CreateCashMovement.Request();
        public int CashMovementDirection { get; set; }
        public FinancePage(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            GetDataForMonthFilter();
            return Task.CompletedTask;
        }

        public RadzenGrid<CashMovementViewModel> CashMovementGrid { get; set; }

        public void GetDataForMonthFilter()
        {
            if (App.ActiveClientWithData == null) return;

            int id = 1;
            MonthsFilterData.Clear();

            foreach (var record in App.ActiveClientWithData.CashMovements)
            {
                var year = record.EventDate.Year;
                var month = record.EventDate.Month;

                if (!MonthsFilterData.Any(x => x.Month == month && x.Year == year))
                {
                    MonthsFilterData.Add(new MonthFilterRecord { Id = id, Month = month, Year = year });
                    id++;
                }
            }

            if (MonthsFilterData.Count != 0)
            {
                MonthsFilterData.OrderBy(x => x.Year + x.Month);
            }
        }

        public void EditCashMovementRow(CashMovementViewModel cash) => CashMovementGrid.EditRow(cash);

        public async Task OnUpdateCashMovementRow(CashMovementViewModel cash)
        {
            try
            {
                var result = await UpdateCashMovement.Update(new UpdateCashMovement.Request
                {
                    Id = cash.Id,
                    Amount = cash.Amount,
                    Name = cash.Name,
                    Expense = cash.Expense,
                    Updated = DateTime.Now,
                    UpdatedBy = App.User.UserName,
                    EventDate = cash.EventDate
                });

                if (App.ActiveClient != null)
                {
                    App.ActiveClientWithData.CashMovements[App.ActiveClientWithData.CashMovements.FindIndex(x => x.Id == result.Id)] = result;
                }

                await CashMovementGrid.Reload();
                GetDataForMonthFilter();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kwota: {result.Amount} zł, została zmieniona.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void SaveCashMovementRow(CashMovementViewModel cash) => CashMovementGrid.UpdateRow(cash);

        public void CancelCashMovementEdit(CashMovementViewModel cash)
        {
            CashMovementGrid.CancelEditRow(cash);
            App.RefreshClientWithData();
        }

        public async Task DeleteCashMovementRow(CashMovementViewModel cash)
        {
            try
            {
                await DeleteCashMovement.Delete(cash.Id);
                App.ActiveClientWithData.CashMovements.RemoveAll(x => x.Id == cash.Id);

                await CashMovementGrid.Reload();
                GetDataForMonthFilter();
                App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Kwota: {cash.Amount} zł, została usunięta.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public async Task SubmitNewCashMovement(CreateCashMovement.Request request)
        {
            try
            {
                request.UpdatedBy = App.User.UserName;

                var result = await CreateCashMovement.Create(App.ActiveClient.Id, App.User.Profile, request);
                NewCashMovement = new CreateCashMovement.Request();

                App.ActiveClientWithData.CashMovements.Add(result);
                await CashMovementGrid.Reload();
                GetDataForMonthFilter();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kwota: {result.Amount} zł, została dodana.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void ActiveCashMovementChange(object value)
        {
            var input = (CashMovementViewModel)value;
            if (input != null)
            {
                App.ActiveClientWithData.SelectedCashMovement = App.ActiveClientWithData.CashMovements.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                App.ActiveClientWithData.SelectedCashMovement = null;
            }
        }

        public void SelectedMonthChange(object value)
        {
            var input = (int?)value;
            if (input != null)
            {
                SelectedMonth = MonthsFilterData.FirstOrDefault(x => x.Id == input);
            }
            else
            {
                SelectedMonth = null;
            }
        }
    }
}
