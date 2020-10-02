using Radzen;
using Radzen.Blazor;
using SWP.Application.LegalSwp.TimeRecords;
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
    public class ProductivityPage : BlazorPageBase
    {
        private CreateTimeRecord CreateTimeRecord => serviceProvider.GetService<CreateTimeRecord>();
        private DeleteTimeRecord DeleteTimeRecord => serviceProvider.GetService<DeleteTimeRecord>();
        private UpdateTimeRecord UpdateTimeRecord => serviceProvider.GetService<UpdateTimeRecord>();

        public LegalBlazorApp App { get; private set; }
        public CreateTimeRecord.Request NewTimeRecord { get; set; } = new CreateTimeRecord.Request();

        public ProductivityPage(IServiceProvider serviceProvider) : base(serviceProvider) { }

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
                var result = await UpdateTimeRecord.Update(new UpdateTimeRecord.Request
                {
                    Id = time.Id,
                    Description = time.Description,
                    Name = time.Name,
                    EventDate = time.EventDate,
                    RecordedHours = time.RecordedHours,
                    RecordedMinutes = time.RecordedMinutes,
                    Updated = DateTime.Now,
                    UpdatedBy = App.User.UserName
                });

                if (App.ActiveClient != null)
                {
                    App.ActiveClientWithData.TimeRecords[App.ActiveClientWithData.TimeRecords.FindIndex(x => x.Id == result.Id)] = result;
                }

                await TimeRecordsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Wpis: {result.Name}, został zmieniony.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void SaveTimeRecordRow(TimeRecordViewModel time) => TimeRecordsGrid.UpdateRow(time);

        public void CancelTimeRecordEdit(TimeRecordViewModel time)
        {
            TimeRecordsGrid.CancelEditRow(time);
            App.RefreshClientWithData();
        }

        public async Task DeleteTimeRecordRow(TimeRecordViewModel time)
        {
            try
            {
                await DeleteTimeRecord.Delete(time.Id);
                App.ActiveClientWithData.TimeRecords.RemoveAll(x => x.Id == time.Id);
                App.ActiveClientWithData.SelectedTimeRecord = null;

                await TimeRecordsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Wpis: {time.Name}, został usunięty.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public async Task SubmitNewTimeRecord(CreateTimeRecord.Request arg)
        {
            if (arg.RecordedTime == new TimeSpan(0,0,0))
            {
                return;
            }

            try
            {
                NewTimeRecord.UpdatedBy = App.User.UserName;

                var result = await CreateTimeRecord.Create(App.ActiveClient.Id, App.User.Profile, NewTimeRecord);
                NewTimeRecord = new CreateTimeRecord.Request();

                App.ActiveClientWithData.TimeRecords.Add(result);
                await TimeRecordsGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Wpis: {result.Name}, został dodany.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void ActiveTimeRecordChange(object value)
        {
            var input = (TimeRecordViewModel)value;
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
