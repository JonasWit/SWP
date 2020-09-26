using Microsoft.Extensions.DependencyInjection;
using Radzen;
using SWP.Application.LegalSwp.Cases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App.ComponentsApps
{
    [UITransientService]
    public class AddCaseComponentApp : ComponentAppBase
    {
        private readonly IServiceProvider serviceProvider;

        public CreateCase.Request NewCase { get; set; } = new CreateCase.Request();
        private CreateCase CreateCase => serviceProvider.GetService<CreateCase>();

        public AddCaseComponentApp(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task CreateNewCase(CreateCase.Request arg)
        {
            try
            {
                NewCase.UpdatedBy = MainApp.User.UserName;
                var result = await CreateCase.Create(MainApp.ActiveClientWithData.Id, MainApp.User.Profile, NewCase);
                NewCase = new CreateCase.Request();

                MainApp.ActiveClientWithData.Cases.Add(result);
                await MainApp.CasesPage.CasesGrid.Reload();
                MainApp.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Sprawa: {result.Name} została dodana.", GeneralViewModel.NotificationDuration);
            }
            catch (Exception ex)
            {
                MainApp.ErrorPage.DisplayMessage(ex);
            }
        }




    }
}
