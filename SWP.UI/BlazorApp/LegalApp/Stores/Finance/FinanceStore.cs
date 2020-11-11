using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.CashMovements;
using SWP.UI.BlazorApp.LegalApp.Stores.Main;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Finance
{
    public class FinanceState
    {
        public CreateCashMovement.Request NewCashMovement { get; set; } = new CreateCashMovement.Request();
        public int CashMovementDirection { get; set; }
        public RadzenGrid<CashMovementViewModel> CashMovementGrid { get; set; }
        public MonthFilterRecord SelectedMonth { get; set; }
        public List<MonthFilterRecord> MonthsFilterData { get; set; } = new List<MonthFilterRecord>();

        public class MonthFilterRecord
        {
            public int Id { get; set; }
            public string DisplayText => $"{Month}-{Year}";
            public int Month { get; set; }
            public int Year { get; set; }
        }
    }

    [UIScopedService]
    public class FinanceStore : StoreBase
    {
        private readonly FinanceState _state;

        public FinanceState GetState() => _state;

        private MainStore _mainStore => _serviceProvider.GetRequiredService<MainStore>();

        public FinanceStore(IServiceProvider serviceProvider, NotificationService notificationService, DialogService dialogService)
            : base(serviceProvider, notificationService, dialogService)
        {
            _state = new FinanceState();
        }

        public void Initialize()
        {
            GetDataForMonthFilter();
        }

        public void GetDataForMonthFilter()
        {
            int id = 1;
            _state.MonthsFilterData.Clear();

            foreach (var record in _mainStore.GetState().ActiveClient.CashMovements)
            {
                var year = record.EventDate.Year;
                var month = record.EventDate.Month;

                if (!_state.MonthsFilterData.Any(x => x.Month == month && x.Year == year))
                {
                    _state.MonthsFilterData.Add(new FinanceState.MonthFilterRecord { Id = id, Month = month, Year = year });
                    id++;
                }
            }

            if (_state.MonthsFilterData.Count != 0)
            {
                _state.MonthsFilterData.OrderBy(x => x.Year + x.Month);
            }
        }

        public void EditCashMovementRow(CashMovementViewModel cash) => _state.CashMovementGrid.EditRow(cash);

        public async Task OnUpdateCashMovementRow(CashMovementViewModel cash)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var updateCashMovement = scope.ServiceProvider.GetRequiredService<UpdateCashMovement>();

                var result = await updateCashMovement.Update(new UpdateCashMovement.Request
                {
                    Id = cash.Id,
                    Amount = cash.Amount,
                    Name = cash.Name,
                    Expense = cash.Expense,
                    Updated = DateTime.Now,
                    UpdatedBy = _mainStore.GetState().User.UserName,
                    EventDate = cash.EventDate
                });

                if (_mainStore.GetState().ActiveClient != null)
                {
                    _mainStore.ReplaceCashMovementFromActiveClient(result);
                }

                await _state.CashMovementGrid.Reload();
                GetDataForMonthFilter();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kwota: {result.Amount} zł, została zmieniona.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        public void SaveCashMovementRow(CashMovementViewModel cash) => _state.CashMovementGrid.UpdateRow(cash);

        public void CancelCashMovementEdit(CashMovementViewModel cash)
        {
            _state.CashMovementGrid.CancelEditRow(cash);
            _mainStore.RefreshActiveClientData();
            BroadcastStateChange();
        }

        public async Task DeleteCashMovementRow(CashMovementViewModel cash)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteCashMovement = scope.ServiceProvider.GetRequiredService<DeleteCashMovement>();

                await deleteCashMovement.Delete(cash.Id);
                _mainStore.RemoveCashMovementFromActiveClient(cash.Id);

                await _state.CashMovementGrid.Reload();
                GetDataForMonthFilter();
                ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Kwota: {cash.Amount} zł, została usunięta.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        public async Task SubmitNewCashMovement(CreateCashMovement.Request request)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var createCashMovement = scope.ServiceProvider.GetRequiredService<CreateCashMovement>();

                request.UpdatedBy = _mainStore.GetState().User.UserName;

                var result = await createCashMovement.Create(_mainStore.GetState().ActiveClient.Id, _mainStore.GetState().User.Profile, request);
                _state.NewCashMovement = new CreateCashMovement.Request();

                _mainStore.AddCashMovementToActiveClient(result);

                if (_state.CashMovementGrid != null)
                {
                    await _state.CashMovementGrid.Reload();
                    GetDataForMonthFilter();
                }

                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kwota: {result.Amount} zł, została dodana.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
            finally
            {
                BroadcastStateChange();
            }
        }

        public void ActiveCashMovementChange(object value)
        {
            var input = (CashMovementViewModel)value;
            if (input != null)
            {
                _mainStore.SetSelectedCashMovement(_mainStore.GetState().ActiveClient.CashMovements.FirstOrDefault(x => x.Id == input.Id));
            }
            else
            {
                _mainStore.SetSelectedCashMovement(null);
            }
        }

        public void SelectedMonthChange(object value)
        {
            var input = (int?)value;
            if (input != null)
            {
                _state.SelectedMonth = _state.MonthsFilterData.FirstOrDefault(x => x.Id == input);
            }
            else
            {
                _state.SelectedMonth = null;
            }
        }
    }
}
