//using Microsoft.Extensions.DependencyInjection;
//using Radzen;
//using SWP.Application.LegalSwp.Cases;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data;
//using SWP.UI.Components.LegalSwpBlazorComponents.App.ComponentsApps;
//using Radzen.Blazor;

//namespace SWP.UI.Components.LegalSwpBlazorComponents.App.CasesComponentsApps
//{
//    [UITransientService]
//    public class CasesGridComponentApp : ComponentAppBase
//    {
//        private GetCase GetCase => serviceProvider.GetService<GetCase>();
//        private UpdateCase UpdateCase => serviceProvider.GetService<UpdateCase>();
//        private DeleteCase DeleteCase => serviceProvider.GetService<DeleteCase>();

//        public RadzenGrid<CaseViewModel> CasesGrid { get; set; }

//        private readonly IServiceProvider serviceProvider;

//        public CasesGridComponentApp(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;

//        public void ReloadCase(int id)
//        {
//            var caseEntity = GetCase.Get(id);
//            MainApp.ActiveClientWithData.Cases.RemoveAll(x => x.Id == id);
//            MainApp.ActiveClientWithData.Cases.Add(caseEntity);
//            MainApp.ActiveClientWithData.Cases = MainApp.ActiveClientWithData.Cases.OrderBy(x => x.Name).ToList();
//            MainApp.ActiveClientWithData.Cases.TrimExcess();
//            MainApp.ActiveClientWithData.SelectedCase = caseEntity;
//        }

//        public void EditCaseRow(CaseViewModel c) => CasesGrid.EditRow(c);

//        public async Task OnUpdateCaseRow(CaseViewModel c)
//        {
//            try
//            {
//                var result = await UpdateCase.Update(new UpdateCase.Request
//                {
//                    Id = c.Id,
//                    CaseType = c.CaseType,
//                    Description = c.Description,
//                    Name = c.Name,
//                    Signature = c.Signature,
//                    UpdatedBy = MainApp.User.UserName
//                });

//                MainApp.ActiveClientWithData.Cases[MainApp.ActiveClientWithData.Cases.FindIndex(x => x.Id == result.Id)] = result;

//                if (MainApp.ActiveClientWithData.SelectedCase.Id == result.Id)
//                {
//                    MainApp.ActiveClientWithData.SelectedCase = MainApp.ActiveClientWithData.Cases.FirstOrDefault(x => x.Id == result.Id);
//                }

//                await CasesGrid.Reload();
//                MainApp.ShowNotification(NotificationSeverity.Success, "Sukces!", $"Sprawa: {result.Name} została zmieniona.", GeneralViewModel.NotificationDuration);
//            }
//            catch (Exception ex)
//            {
//                MainApp.ErrorPage.DisplayMessage(ex);
//            }
//        }

//        public void SaveCaseRow(CaseViewModel c) => CasesGrid.UpdateRow(c);

//        public void CancelEditCaseRow(CaseViewModel c)
//        {
//            CasesGrid.CancelEditRow(c);
//            MainApp.RefreshClientWithData();
//        }

//        public async Task DeleteCaseRow(CaseViewModel c)
//        {
//            try
//            {
//                await DeleteCase.Delete(c.Id);

//                MainApp.ActiveClientWithData.Cases.RemoveAll(x => x.Id == c.Id);
//                await CasesGrid.Reload();
//                MainApp.ShowNotification(NotificationSeverity.Warning, "Sukces!", $"Sprawa: {c.Name} została usunięta.", GeneralViewModel.NotificationDuration);

//            }
//            catch (Exception ex)
//            {
//                MainApp.ErrorPage.DisplayMessage(ex);
//            }
//        }

//        public void ActiveCaseChange(object value)
//        {
//            var input = (CaseViewModel)value;
//            if (value != null)
//            {
//                MainApp.ActiveClientWithData.SelectedCase = MainApp.ActiveClientWithData.Cases.FirstOrDefault(x => x.Id == input.Id);
//            }
//            else
//            {
//                MainApp.ActiveClientWithData.SelectedCase = null;
//            }
//        }
//    }
//}
