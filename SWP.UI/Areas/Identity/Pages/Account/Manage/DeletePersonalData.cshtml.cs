using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SWP.Application.LegalSwp.Clients;
using SWP.Application.PortalCustomers;
using SWP.Domain.Enums;
using SWP.UI.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly DeleteClient _deleteClient;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly DeleteBillingRecord _deleteBillingRecord;

        public DeletePersonalDataModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            DeleteClient deleteClient,
            ILogger<DeletePersonalDataModel> logger,
            DeleteBillingRecord deleteBillingRecord)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _deleteClient = deleteClient;
            _logger = logger;
            _deleteBillingRecord = deleteBillingRecord;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var claims = await _userManager.GetClaimsAsync(user) as List<Claim>;

            if (claims.Any(x => x.Type == "Root" && x.Value == "Creator"))
            {
                return NotFound($"Unable to delete Creator!");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            var profileClaim = claims.FirstOrDefault(x => x.Type == ClaimType.Profile.ToString());
            var applicationClaims = claims.Where(x => x.Type == ClaimType.Application.ToString());
            var rootClientClaim = claims.FirstOrDefault(x => x.Type == ClaimType.Status.ToString() && x.Value == UserStatus.RootClient.ToString());
            var usersWithTheSameProfile = await _userManager.GetUsersForClaimAsync(profileClaim);

            var claimsToRemove = new List<Claim>
            {
                profileClaim
            };

            claimsToRemove.AddRange(applicationClaims);

            try
            {
                if (rootClientClaim != null)
                {
                    //todo: delete also related accounts! - przemyslec to
                    //todo: skasowac też wszystkie requesty tego usera
                    await _deleteBillingRecord.DeleteBillingDetail(user.Id);

                    //Delete all clients data connected to profile
                    await _deleteClient.Delete(profileClaim.Value);

                    //Remove Profile form all related non-Root users
                    foreach (var userWithTheSameProfile in usersWithTheSameProfile)
                    {
                        if (userWithTheSameProfile.Id != user.Id)
                        {
                            var actionResult = await _userManager.RemoveClaimsAsync(userWithTheSameProfile, claimsToRemove);

                            if (actionResult.Succeeded)
                            {
                                _logger.LogInformation(LogTags.PortalIdentityLogPrefix + "Profile removed for user {userName} clean up initiated by {rootUserName}", userWithTheSameProfile.UserName, user.UserName);
                            }
                            else
                            {
                                _logger.LogInformation(LogTags.PortalIdentityLogPrefix + "Failed to remove profile for user {userName}", userWithTheSameProfile.UserName, user.UserName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogTags.PortalIdentityLogPrefix + "Error when cleaning up data for Root user {userName}", user.UserName);
            }

            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogError(LogTags.PortalIdentityLogPrefix + "Error when deleting Root user {userName}", user.UserName);
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation(LogTags.PortalIdentityLogPrefix + "User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
    }
}
