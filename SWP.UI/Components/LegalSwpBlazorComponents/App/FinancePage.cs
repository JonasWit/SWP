using Radzen;
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
        private readonly IServiceProvider serviceProvider;
        private CreateCashMovement CreateCashMovement => serviceProvider.GetService<CreateCashMovement>();
        private DeleteCashMovement DeleteCashMovement => serviceProvider.GetService<DeleteCashMovement>();
        private UpdateCashMovement UpdateCashMovement => serviceProvider.GetService<UpdateCashMovement>();

        public LegalBlazorApp App { get; private set; }
        public CreateCashMovement.Request NewCashMovement { get; set; } = new CreateCashMovement.Request();
        public int CashMovementDirection { get; set; }

        public FinancePage(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            return Task.CompletedTask;
        }

        public RadzenGrid<CashMovementViewModel> CashMovementGrid { get; set; }

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
                    Updated = DateTime.Now,
                    UpdatedBy = App.User.UserName,
                    EventDate = cash.EventDate
                });

                if (App.ActiveClient != null)
                {
                    App.ActiveClientWithData.CashMovements[App.ActiveClientWithData.CashMovements.FindIndex(x => x.Id == result.Id)] = result;
                }

                await CashMovementGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kwota: {result.Amount} zł, została zmieniona.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
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
                App.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Kwota: {cash.Amount} zł, została usunięta.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public async Task SubmitNewCashMovement(CreateCashMovement.Request arg)
        {
            try
            {
                NewCashMovement.UpdatedBy = App.User.UserName;

                var result = await CreateCashMovement.Create(App.ActiveClient.Id, App.User.Profile, NewCashMovement);
                NewCashMovement = new CreateCashMovement.Request();

                App.ActiveClientWithData.CashMovements.Add(result);
                await CashMovementGrid.Reload();
                App.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Kwota: {result.Amount} zł, została dodana.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                App.ErrorPage.DisplayMessage(ex);
            }
        }

        public void RowRender(RowRenderEventArgs<CashMovementViewModel> args)
        {
            //args.Attributes.Add("style", $"font-weight: {(args.Data.Amount < 0 ? "bold" : "normal")};");
        }

        public void CellRender(CellRenderEventArgs<CashMovementViewModel> args)
        {
            //if (args.Column.Property == "Amount")
            //{
            //    args.Attributes.Add("style", $"background-color: {(args.Data.Amount < 0 ? "#ff6d41" : "white")};");
            //}
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
    }
}
