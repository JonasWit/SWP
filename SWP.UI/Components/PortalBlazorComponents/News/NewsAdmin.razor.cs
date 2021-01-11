using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using SWP.UI.BlazorApp;
using SWP.UI.BlazorApp.PortalApp.Stores.News.NewsAdmin;
using SWP.UI.BlazorApp.PortalApp.Stores.News.NewsAdmin.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SWP.UI.Components.PortalBlazorComponents.News
{
    public partial class NewsAdmin : IDisposable
    {
        [Inject]
        public NewsAdminStore Store { get; set; }
        [Inject]
        public IActionDispatcher ActionDispatcher { get; set; }

        [Parameter]
        public int? PostId { get; set; }
        [Parameter]
        public string UserId { get; set; }

        public void Dispose()
        {
            Store.RemoveStateChangeListener(UpdateView);
        }

        private void UpdateView()
        {
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            Store.EnableLoading();

            Store.Initialize(UserId, PostId);
            Store.AddStateChangeListener(UpdateView);

            Store.DisableLoading();
        }

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            foreach (IBrowserFile imageFile in e.GetMultipleFiles(1))
            {
                try
                {
                    //Store.EnableLoading("Uploading...");

                    var buffer = new byte[imageFile.Size];

                    var cleanStream = imageFile.OpenReadStream(5242880);
                    await cleanStream.ReadAsync(buffer);
                    Store.GetState().DataModel.ImageStream = buffer;
                    Store.GetState().DataModel.ImageName = imageFile.Name;
                }
                catch (Exception ex)
                {
                    
                }
                finally
                {
                    //Store.DisableLoading();
                }
            }
        }

        private void OnProgress(UploadProgressArgs args, string name)
        {
            var info = $"% '{name}' / {args.Loaded} of {args.Total} bytes.";
            var progress = args.Progress;

            if (args.Progress == 100)
            {
       
                foreach (var file in args.Files)
                {
   
                }
            }
        }

        #region Actions

        private void SubmitNews() => ActionDispatcher.Dispatch(new PostNewsAction());







        #endregion


    }
}
