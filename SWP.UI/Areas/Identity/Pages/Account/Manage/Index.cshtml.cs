using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP.Domain.Models.Portal;

namespace SWP.UI.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Display(Name = "Nazwa użytkownika (Jest to adres email użyty do rejestracji)")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Numer Kontaktowy")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Imię")]
            public string Name { get; set; }

            [Display(Name = "Nazwisko")]
            public string Surname { get; set; }

            [Display(Name = "Adres")]
            public string Address { get; set; }

            [Display(Name = "Adres Korespondencyjny")]
            public string AddressCorrespondence { get; set; }

            [Display(Name = "Miasto")]
            public string City { get; set; }

            [Display(Name = "Województwo")]
            public string Vivodership { get; set; }

            [Display(Name = "Kraj")]
            public string Country { get; set; }

            [Display(Name = "Kod Pocztowy")]
            public string PostCode { get; set; }

            [Display(Name = "Pełna Nazwa Firmy")]
            public string CompanyFullName { get; set; }

            [Display(Name = "NIP")]
            [StringLength(10, ErrorMessage = "NIP musi składać się z 10 znaków!")]
            [RegularExpression(@"[0-9]{10}$", ErrorMessage = "Nieprawidłowy NIP!")]
            public string NIP { get; set; }

            [Display(Name = "REGON")]
            public string REGON { get; set; }

            [Display(Name = "PESEL")]
            [StringLength(11, ErrorMessage = "PESEL musi składać się z 11 znaków!")]
            public string PESEL { get; set; }

            [Display(Name = "KRS")]
            public string KRS { get; set; }


            public static implicit operator BillingDetails(InputModel input) =>
                new BillingDetails
                {
                    Address = input.Address,
                    AddressCorrespondence = input.AddressCorrespondence,
                    City = input.City,
                    CompanyFullName = input.CompanyFullName,
                    Country = input.Country,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                   
                };
        }

        /// <summary>
        /// Load All Data Related to User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var claims = await _userManager.GetClaimsAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            //todo: save billing data in database

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
