using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SWP.Application.LegalSwp.Clients;
using SWP.Application.PortalCustomers;
using SWP.Domain.Enums;
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
        private readonly ClearCustomerRelatedData _clearCustomerRelatedData;
        private readonly ILogger<DeletePersonalDataModel> _logger;

        public DeletePersonalDataModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            DeleteClient deleteClient,
            ILogger<DeletePersonalDataModel> logger,
            ClearCustomerRelatedData clearCustomerRelatedData)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _deleteClient = deleteClient;
            _clearCustomerRelatedData = clearCustomerRelatedData;
            _logger = logger;
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
            var applicationClaim = claims.FirstOrDefault(x => x.Type == ClaimType.Application.ToString());
            var rootClientClaim = claims.FirstOrDefault(x => x.Type == ClaimType.Status.ToString() && x.Value == UserStatus.RootClient.ToString());
            var usersWithTheSameProfile = await _userManager.GetUsersForClaimAsync(profileClaim);

            try
            {
                if (rootClientClaim != null)
                {
                    await _clearCustomerRelatedData.CleanUp(user.Id);

                    //Delete all clients data connected to profile
                    await _deleteClient.Delete(profileClaim.Value);

                    //Remove Profile form all related non-Root users
                    foreach (var userWithTheSameProfile in usersWithTheSameProfile)
                    {
                        if (userWithTheSameProfile.Id != user.Id)
                        {
                            var actionResult = await _userManager.RemoveClaimsAsync(userWithTheSameProfile, new List<Claim> { profileClaim, applicationClaim });

                            if (actionResult.Succeeded)
                            {
                                //todo:add logging!

                                //await _portalLogger.CreateLogRecord(new CreateLogRecord.Request
                                //{
                                //    Message = $"Success! Profile {profileClaim.Value} for User {userWithTheSameProfile.UserName} Deleted!",
                                //    UserId = user.Id,
                                //});
                            }
                            else
                            {
                                //await _portalLogger.CreateLogRecord(new CreateLogRecord.Request
                                //{
                                //    Message = $"Issue! Profile {profileClaim.Value} for User {userWithTheSameProfile.UserName} Not Deleted!",
                                //    UserId = user.Id,
                                //});
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //todo:add logging!

                //await _portalLogger.CreateLogRecord(new CreateLogRecord.Request
                //{
                //    Message = $"Issue during Delete Data Request from User! - {ex.Message}",
                //    UserId = user.Id,
                //    StackTrace = ex.StackTrace
                //});
            }

            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);

            if (!result.Succeeded)
            {
                //todo:add logging!

                //await _portalLogger.CreateLogRecord(new CreateLogRecord.Request
                //{
                //    Message = $"Unexpected error occurred deleting user with ID '{userId}'",
                //    UserId = user.Id,
                //});

                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
    }
}
