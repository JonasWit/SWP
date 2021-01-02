using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Radzen;
using SWP.Domain.Enums;
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

        public List<Recipient> Recipients { get; set; } = new List<Recipient>();
        public IEnumerable<string> SelectedRecipients { get; set; }
        public IEnumerable<int> UserTypes { get; set; }
        public IEnumerable<int> ApplicationTypes { get; set; }
    }

    public class Recipient
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public UserStatus Status { get; set; }
        public List<ApplicationType> AppClaims { get; set; } = new List<ApplicationType>();

        public string ListApps => AppClaims.Count != 0 ? string.Join(", ", AppClaims.Select(x => x.ToString())) : "No Apps";
        public string DisplayName => $"{UserName} [Status: {Status}] [Apps: {ListApps}]"; 
    }

    [UIScopedService]
    public class CommunicationStore : StoreBase<CommunicationState>
    {
        private readonly ILogger<CommunicationStore> _logger;

        StatusBarStore StatusBarStore => _serviceProvider.GetRequiredService<StatusBarStore>();
        UserManager<IdentityUser> UserManager => _serviceProvider.GetService<UserManager<IdentityUser>>();
        ApplicationStore AppStore => _serviceProvider.GetRequiredService<ApplicationStore>();

        public CommunicationStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService, ILogger<CommunicationStore> logger)
            : base(serviceProvider, actionDispatcher, notificationService)
        {
            _logger = logger;
        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case SendAction.Send:
                    await Send();
                    break;
                case FilterAction.Filter:
                    Filter();
                    break;
                case ClearSelectionAction.ClearSelection:
                    ClearSelection();
                    break;
                default:
                    break;
            }
        }

        #region Communication

        public async Task GetRecipients()
        {
            _state.Recipients = UserManager.Users.Select(x => new Recipient
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                AppClaims = new List<ApplicationType>()
            }).ToList();

            foreach (var recipient in _state.Recipients)
            {
                var claims = await UserManager.GetClaimsAsync(await UserManager.FindByIdAsync(recipient.Id));
                var userStatus = claims.FirstOrDefault(x => x.Type == ClaimType.Status.ToString());

                if (claims.Any(x => x.Type == ClaimType.Application.ToString()))
                {
                    var appClaims = claims.Where(x => x.Type == ClaimType.Application.ToString()).Select(x => x.Value).ToList();

                    foreach (var ac in appClaims)
                    {
                        if (Enum.TryParse(ac, out ApplicationType appEn))
                        {
                            recipient.AppClaims.Add(appEn);
                        }
                    }
                }

                if (userStatus != null)
                {
                    if (Enum.TryParse(userStatus.Value, out UserStatus userEn))
                    {
                        recipient.Status = userEn;
                    }
                }
                else
                {
                    recipient.Status = UserStatus.Undefined;
                }
            }
        }

        public void Filter()
        {
            var selectedRecipients = new List<string>();

            if (_state.UserTypes != null)
            {
                var selectedUserTypes = _state.UserTypes.ToList();

                foreach (var userTypeNo in selectedUserTypes)
                {
                    var enumt = (UserStatus)Enum.GetValues(typeof(UserStatus)).GetValue(userTypeNo);
                    selectedRecipients.AddRange(_state.Recipients.Where(x => x.Status == enumt).Select(x => x.Id).ToList());
                }
            }

            if (_state.ApplicationTypes != null)
            {
                var selectedApplicationTypes = _state.ApplicationTypes.ToList();

                foreach (var userTypeNo in selectedApplicationTypes)
                {
                    var enumt = (ApplicationType)Enum.GetValues(typeof(ApplicationType)).GetValue(userTypeNo);
                    selectedRecipients.AddRange(_state.Recipients.Where(x => x.AppClaims.Contains(enumt)).Select(x => x.Id).ToList());
                }
            }

            _state.SelectedRecipients = selectedRecipients;
            BroadcastStateChange();
        }

        public void ClearSelection()
        {
            _state.UserTypes = null;
            _state.ApplicationTypes = null;
            _state.SelectedRecipients = new List<string>();
            BroadcastStateChange();
        }

        private Recipient FindRecipient(string id) => _state.Recipients.FirstOrDefault(x => x.Id == id);

        public async Task Send()
        {
            using var scope = _serviceProvider.CreateScope();
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

            var title = _state.Title;
            var body = _state.Body;

            try
            {
                var recipients = _state.Recipients.Where(x => _state.SelectedRecipients.Contains(x.Id));
                foreach (var recipient in recipients)
                {
                    try
                    {
                        await emailSender.SendEmailAsync(recipient.Email, _state.Title, _state.Body);

                        _logger.LogInformation("Seccess: Email {emailTitle} sent to {email}", _state.Title, recipient.Email);
                        StatusBarStore.UpdateLogWindow($"Seccess: Email {_state.Title} sent to {recipient.Email}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Exception: Failed to send email {emailTitle} to {email}", _state.Title, recipient.Email);
                        StatusBarStore.UpdateLogWindow($"Exception: Failed to send email {_state.Title} to {recipient.Email}");
                    }
                }
            }
            catch (Exception ex)
            {
                StatusBarStore.UpdateLogWindow($"Exception: {ex.Message} - logged.");
                ShowErrorPage(ex);
            }
        }

        #endregion

        private void ShowErrorPage(Exception ex) => AppStore.ShowErrorPage(ex);
    }
}
