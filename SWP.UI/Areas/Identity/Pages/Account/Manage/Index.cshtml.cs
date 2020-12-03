using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP.Application.PortalCustomers;
using SWP.Domain.Models.Portal;
using SWP.UI.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SWP.UI.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly GetBillingRecord _getBillingRecord;
        private readonly UpdateBillingRecord _updateBillingRecord;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            GetBillingRecord getBillingRecord,
            UpdateBillingRecord updateBillingRecord)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _getBillingRecord = getBillingRecord;
            _updateBillingRecord = updateBillingRecord;
        }

        [Display(Name = "Nazwa użytkownika (Jest to adres email użyty do rejestracji)")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string UserId { get; set; }
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

            [Display(Name = "KRS")]
            public string KRS { get; set; }

            public static implicit operator BillingDetail(InputModel input) =>
                new BillingDetail
                {
                    UserId = input.UserId,
                    Address = input.Address,
                    AddressCorrespondence = input.AddressCorrespondence,
                    City = input.City,
                    CompanyFullName = input.CompanyFullName,
                    Country = input.Country,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    KRS = input.KRS,
                    Name = input.Name,
                    NIP = input.NIP,
                    PhoneNumber = input.PhoneNumber,
                    PostCode = input.PostCode,
                    REGON = input.REGON,
                    Surname = input.Surname,
                    Vivodership = input.Vivodership,
                };

            public static implicit operator InputModel(BillingDetail input) =>
                new InputModel
                {
                    UserId = input.UserId,
                    Address = input.Address,
                    AddressCorrespondence = input.AddressCorrespondence,
                    City = input.City,
                    CompanyFullName = input.CompanyFullName,
                    Country = input.Country,
                    KRS = input.KRS,
                    Name = input.Name,
                    NIP = input.NIP,
                    PhoneNumber = input.PhoneNumber,
                    PostCode = input.PostCode,
                    REGON = input.REGON,
                    Surname = input.Surname,
                    Vivodership = input.Vivodership,
                };
        }

        /// <summary>
        /// Load All Data Related to User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task LoadAsync(IdentityUser user)
        {
            Username = await _userManager.GetUserNameAsync(user);
            var data = _getBillingRecord.GetDetails(user.Id);

            if (data == null)
            {
                Input = new InputModel();
            }
            else
            {
                Input = data;
            }
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
            
            Input.UserId = user.Id;

            try
            {
                Input = await _updateBillingRecord.Update(Input);
                StatusMessage = "Dane zostały zaktualizowane";
            }
            catch (Exception ex)
            {
                //await _portalLogger.CreateLogRecord($"{user.UserName}-{user.Id}", $"Exception when updating Billing Detials: {ex.Message}", ex.StackTrace);
                StatusMessage = "Wystąpił błąd, spróbuj jeszcze raz";
            }

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
            return RedirectToPage();
        }
    }
}
