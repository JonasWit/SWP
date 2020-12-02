using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.CashMovements;
using SWP.UI.BlazorApp.LegalApp.Stores.Finance.Action;
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
        public CashMovementViewModel SelectedCashMovement { get; set; }
        public List<CashMovementViewModel> CashMovements { get; set; } = new List<CashMovementViewModel>();

        public class MonthFilterRecord
        {
            public int Id { get; set; }
            public string DisplayText => $"{Month}-{Year}";
            public int Month { get; set; }
            public int Year { get; set; }
        }
    }

    [UIScopedService]
    public class FinanceStore : StoreBase<FinanceState>
    {
        private MainStore _mainStore => _serviceProvider.GetRequiredService<MainStore>();

        public FinanceStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, DialogService dialogService)
            : base(serviceProvider, actionDispatcher, notificationService, dialogService)
        {

        }

        public void Initialize()
        {
            GetDataForMonthFilter();
            GetCashMovements(_mainStore.GetState().ActiveClient.Id);
        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case EditCashMovementRowAction.EditCashMovementRow:
                    var editCashMovementRowAction = (EditCashMovementRowAction)action;
                    EditCashMovementRow(editCashMovementRowAction.Arg);
                    break;
                case OnUpdateCashMovementRowAction.OnUpdateCashMovementRow:
                    var onUpdateCashMovementRowAction = (OnUpdateCashMovementRowAction)action;
                    await OnUpdateCashMovementRow(onUpdateCashMovementRowAction.Arg);
                    break;
                default:
                    break;
            }
        }

        private void GetCashMovements(int clientId)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var getCashMovements = scope.ServiceProvider.GetRequiredService<GetCashMovements>();

                _state.CashMovements = getCashMovements.Get(clientId).Select(x => (CashMovementViewModel)x).ToList();
            }
            catch (Exception ex)
            {
                _mainStore.ShowErrorPage(ex).GetAwaiter();
            }
        }

        private void GetDataForMonthFilter()
        {
            int id = 1;
            _state.MonthsFilterData.Clear();

            foreach (var record in _state.CashMovements)
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

        private void SetSelectedCashMovement(CashMovementViewModel entity) => _state.SelectedCashMovement = entity;

        private void EditCashMovementRow(CashMovementViewModel cash) => _state.CashMovementGrid.EditRow(cash);

        private async Task OnUpdateCashMovementRow(CashMovementViewModel cash)
        {
            try
            {
                if (Math.Abs(cash.Amount) == 0)
                {
                    ShowNotification(NotificationSeverity.Warning, "Uwaga!", $"Kwota nie może być zerem.", GeneralViewModel.NotificationDuration);
                    CancelCashMovementEdit(cash);
                    return;
                }

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

                _state.CashMovements[_state.CashMovements.FindIndex(x => x.Id == result.Id)] = result;

                await _state.CashMovementGrid.Reload();
                GetDataForMonthFilter();
                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kwota: {result.Amount} zł, została zmieniona.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
        }

        public void SaveCashMovementRow(CashMovementViewModel cash) => _state.CashMovementGrid.UpdateRow(cash);

        public void CancelCashMovementEdit(CashMovementViewModel cash)
        {
            _state.CashMovementGrid.CancelEditRow(cash);
            GetCashMovements(_mainStore.GetState().ActiveClient.Id);
            BroadcastStateChange();
        }

        public async Task DeleteCashMovementRow(CashMovementViewModel cash)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var deleteCashMovement = scope.ServiceProvider.GetRequiredService<DeleteCashMovement>();

                await deleteCashMovement.Delete(cash.Id);
                _state.CashMovements.RemoveAll(x => x.Id == cash.Id);

                await _state.CashMovementGrid.Reload();
                GetDataForMonthFilter();
                ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Kwota: {cash.Amount} zł, została usunięta.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
        }

        public async Task SubmitNewCashMovement(CreateCashMovement.Request request)
        {
            try
            {
                if (Math.Abs(request.Amount) == 0)
                {
                    ShowNotification(NotificationSeverity.Warning, "Uwaga!", $"Kwota nie może być zerem.", GeneralViewModel.NotificationDuration);
                    return;
                }

                using var scope = _serviceProvider.CreateScope();
                var createCashMovement = scope.ServiceProvider.GetRequiredService<CreateCashMovement>();

                request.UpdatedBy = _mainStore.GetState().User.UserName;

                var result = await createCashMovement.Create(_mainStore.GetState().ActiveClient.Id, _mainStore.GetState().User.Profile, request);

                _state.NewCashMovement = new CreateCashMovement.Request();
                _state.CashMovements.Add(result);

                if (_state.CashMovementGrid != null)
                {
                    await _state.CashMovementGrid.Reload();
                    GetDataForMonthFilter();
                }

                ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kwota: {result.Amount} zł, została dodana.", GeneralViewModel.NotificationDuration);
                BroadcastStateChange();
            }
            catch (Exception ex)
            {
                await _mainStore.ShowErrorPage(ex);
            }
        }

        public void ActiveCashMovementChange(object value)
        {
            var input = (CashMovementViewModel)value;
            if (input != null)
            {
                SetSelectedCashMovement(_state.CashMovements.FirstOrDefault(x => x.Id == input.Id));
            }
            else
            {
                SetSelectedCashMovement(null);
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

        public override void CleanUpStore()
        {
            _state.SelectedMonth = null;
            _state.SelectedCashMovement = null;
        }

        public override void RefreshSore()
        {
            GetDataForMonthFilter();
            GetCashMovements(_mainStore.GetState().ActiveClient.Id);
        }
    }
}
