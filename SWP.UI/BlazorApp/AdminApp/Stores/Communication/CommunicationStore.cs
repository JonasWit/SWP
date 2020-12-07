using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using SWP.UI.BlazorApp.AdminApp.Stores.Application;
using SWP.UI.BlazorApp.AdminApp.Stores.Communication.Actions;
using SWP.UI.BlazorApp.AdminApp.Stores.StatusLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Communication
{
    public class CommunicationState
    {
        public string Body { get; set; }
        public string Title { get; set; }
        public List<string> Recipients { get; set; }
    }

    [UIScopedService]
    public class CommunicationStore : StoreBase<CommunicationState>
    {
        StatusBarStore StatusBarStore => _serviceProvider.GetRequiredService<StatusBarStore>();

        ApplicationStore AppStore => _serviceProvider.GetRequiredService<ApplicationStore>();

        public CommunicationStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService)
            : base(serviceProvider, actionDispatcher, notificationService) { }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case SendAction.Send:
                    await Send();
                    break;
                default:
                    break;
            }
        }

        public async Task Send()
        {
            using var scope = _serviceProvider.CreateScope();
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

            var title = _state.Title;
            var body = _state.Body;

            try
            {




                //await emailSender.SendEmailAsync(Input.Email, "Potwierdź adres Email", $"<a href=\"{callbackUrl}\">Potwierdzam</a>.");
            }
            catch (Exception ex)
            {
                StatusBarStore.UpdateLogWindow($"Exception: {ex.Message} - logged.");
                ShowErrorPage(ex);
            }

        }

        private void ShowErrorPage(Exception ex) => AppStore.ShowErrorPage(ex);

        public override void CleanUpStore()
        {
      
        }

        public override void RefreshSore()
        {
      
        }
    }
}
