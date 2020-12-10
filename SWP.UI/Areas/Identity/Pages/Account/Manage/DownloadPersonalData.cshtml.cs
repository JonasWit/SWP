using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SWP.Application.PortalCustomers;
using SWP.Domain.Models.Portal;
using SWP.UI.Utilities;

namespace SWP.UI.Areas.Identity.Pages.Account.Manage
{
    public class DownloadPersonalDataModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<DownloadPersonalDataModel> _logger;
        private readonly GetBillingRecord _getBillingRecord;

        public DownloadPersonalDataModel(
            UserManager<IdentityUser> userManager,
            ILogger<DownloadPersonalDataModel> logger,
            GetBillingRecord getBillingRecord)
        {
            _userManager = userManager;
            _logger = logger;
            _getBillingRecord = getBillingRecord;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation(LogTags.PortalIdentityLogPrefix + "User with ID '{UserId}' asked for their personal data.", _userManager.GetUserId(User));

            var billingData = _getBillingRecord.GetDetails(user.Id);

            // Only include personal data for download
            var personalData = new Dictionary<string, string>();
            var billingPersonalDataProps = typeof(BillingDetail).GetProperties().Where( prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));

            // Only include personal data for download
            var personalDataProps = typeof(IdentityUser).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));

            foreach (var p in personalDataProps)
            {
                personalData.Add($"Dane_Konta-{p.Name}", p.GetValue(user)?.ToString() ?? "null");
            }

            if (billingData != null)
            {
                foreach (var p in billingPersonalDataProps)
                {
                    personalData.Add($"Dane_Klienta-{p.Name}", p.GetValue(billingData)?.ToString() ?? "null");
                }
            }

            var logins = await _userManager.GetLoginsAsync(user);
            foreach (var l in logins)
            {
                personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
            }

            Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
            return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData), "application/json");
        }
    }
}
