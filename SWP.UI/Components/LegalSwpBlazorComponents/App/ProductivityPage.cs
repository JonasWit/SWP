using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.TimeRecords;
using SWP.UI.BlazorApp;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class ProductivityPage : BlazorPageBase
    {
        private readonly CreateTimeRecord createTimeRecord;

        public LegalBlazorApp App { get; private set; }
        public CreateTimeRecord.Request NewTimeRecord { get; set; } = new CreateTimeRecord.Request();
        public int CashMovementDirection { get; set; }

        public ProductivityPage(
            CreateTimeRecord createTimeRecord)
        {
            this.createTimeRecord = createTimeRecord;
        }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            return Task.CompletedTask;
        }

        public RadzenGrid<TimeRecordViewModel> TimeRecordsGrid { get; set; }

        public void EditTimeRecordRow(TimeRecordViewModel time) => TimeRecordsGrid.EditRow(time);

        public async Task OnUpdateTimeRecordRow(TimeRecordViewModel time)
        {
            try
            {
                //var result = await updateCashMovement.Update(new UpdateCashMovement.Request
                //{
                //    Id = cash.Id,
                //    Amount = cash.Amount,
                //    Name = cash.Name,
                //    Updated = DateTime.Now,
                //    UpdatedBy = App.User.UserName
                //});

                //if (App.ActiveClient != null)
                //{
                //    App.ActiveClientWithData.CashMovements[App.ActiveClientWithData.CashMovements.FindIndex(x => x.Id == result.Id)] = result;
                //}

                //await CashMovementGrid.Reload();
                //App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kwota: {result.Amount} zł, została zmieniona.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void SaveTimeRecordRow(TimeRecordViewModel time) => TimeRecordsGrid.UpdateRow(time);

        public void CancelTimeRecordEdit(TimeRecordViewModel time)
        {
            //CashMovementGrid.CancelEditRow(cash);
            //App.RefreshClientWithData();
        }

        public async Task DeleteTimeRecordRow(TimeRecordViewModel time)
        {
            try
            {
                //await deleteCashMovement.Delete(cash.Id);
                //App.ActiveClientWithData.CashMovements.RemoveAll(x => x.Id == cash.Id);

                //await CashMovementGrid.Reload();
                //App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Kwota: {cash.Amount} zł, została usunięta.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public async Task SubmitNewTimeRecord(CreateTimeRecord.Request arg)
        {
            if (arg.RecordedTime.TimeOfDay == new TimeSpan(0,0,0))
            {
                return;
            }

            try
            {
                //NewCashMovement.UpdatedBy = App.User.UserName;

                //var result = await createCashMovement.Create(App.ActiveClient.Id, App.User.Profile, NewCashMovement);
                //NewCashMovement = new CreateCashMovement.Request();

                //App.ActiveClientWithData.CashMovements.Add(result);
                //await CashMovementGrid.Reload();
                //App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kwota: {result.Amount} zł, została dodana.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void ActiveTimeRecordChange(object value)
        {
            var input = (CashMovementViewModel)value;
            if (value != null)
            {
                App.ActiveClientWithData.SelectedTimeRecord = App.ActiveClientWithData.TimeRecords.FirstOrDefault(x => x.Id == input.Id);
            }
            else
            {
                App.ActiveClientWithData.SelectedTimeRecord = null;
            }
        }
    }
}
