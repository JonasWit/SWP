using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SWP.Application.PortalCustomers;
using SWP.Domain.Enums;
using SWP.Domain.Infrastructure.Portal;
using SWP.UI.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.Areas.Identity.Pages.Account.Manage
{
    public class ApplicationsProfileModel : PageModel
    {
        [BindProperty]
        //[StringLength(20, ErrorMessage = "Nazwa Profilu musi sk³adaæ siê z 5 do 20 znaków!", MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{5,20}$", ErrorMessage = "Nazwa musi sk³adaæ siê z 5 do 20 liter lub cyfr.")]
        [Display(Name = "Nazwa Profilu")]
        public string ProfileName { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ChangeProfileName _changeProfileName;
        private readonly ILogger<ApplicationsProfileModel> _logger;
        private readonly IIdentityExtendedManager _identityExtendedManager;

        public ApplicationsProfileModel(
            UserManager<IdentityUser> userManager, 
            ChangeProfileName changeProfileName,
            ILogger<ApplicationsProfileModel> logger,
            IIdentityExtendedManager identityExtendedManager)
        {
            _userManager = userManager;
            _changeProfileName = changeProfileName;
            _logger = logger;
            _identityExtendedManager = identityExtendedManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var claims = await _userManager.GetClaimsAsync(user) as List<Claim>;
            var profileClaim = claims.FirstOrDefault(x => x.Type == ClaimType.Profile.ToString());
            ProfileName = profileClaim.Value;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (_identityExtendedManager.ClaimExists(ClaimType.Profile.ToString(), ProfileName))
            {       
                StatusMessage = $"Profil: {ProfileName} ju¿ istanienie, wybierz inn¹ nazwê.";
                return RedirectToPage();
            }

            var user = await _userManager.GetUserAsync(User);
            var claims = await _userManager.GetClaimsAsync(user) as List<Claim>;
            var oldProfileClaim = claims.FirstOrDefault(x => x.Type == ClaimType.Profile.ToString());

            try
            {
                var result = await _changeProfileName.Change(oldProfileClaim, ProfileName);
                StatusMessage = "Nazwa Profilu zosta³a zmieniona.";
                _logger.LogInformation(LogTags.PortalIdentityLogPrefix + "Profile name changed for {userName}, from {oldName} to {newName}", user.UserName, oldProfileClaim.Value, ProfileName);
            }
            catch (Exception ex)
            {
                StatusMessage = "Wyst¹pi³ b³¹d przy zmianie nazwy profilu, spróbuj jeszcze raz.";
                _logger.LogError(ex, LogTags.PortalIdentityLogPrefix + "Error when changing profile name by {userName}", user.UserName);
            }

            return RedirectToPage();
        }
    }
}
