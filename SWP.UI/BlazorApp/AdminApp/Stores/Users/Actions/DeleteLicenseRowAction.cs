using SWP.Domain.Models.Portal;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Users.Actions
{
    public class DeleteLicenseRowAction : IAction
    {
        public const string DeleteLicenseRow = "DELETE_LICENSE_ROW";
        public string Name => DeleteLicenseRow;

        public UserLicense Arg { get; set; }
    }
}
