using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SWP.Application.News;
using SWP.UI.BlazorApp.PortalApp.Stores.News.NewsAdmin.Actions;
using SWP.UI.Pages.News.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.News.NewsAdmin
{
    public class NewsAdminState
    {
        public NewsViewModel DataModel { get; set; } = new NewsViewModel();
        public string ActiveUserId { get; set; }
    }

    [UIScopedService]
    public class NewsAdminStore : StoreBase<NewsAdminState>
    {
        private readonly ILogger<NewsAdminStore> _logger;
        private readonly NavigationManager _navigationManager;

        public NewsAdminStore(
            IServiceProvider serviceProvider,
            IActionDispatcher actionDispatcher,
            ILogger<NewsAdminStore> logger,
            NavigationManager navigationManager)
            : base(serviceProvider, actionDispatcher)
        {
            _logger = logger;
            _navigationManager = navigationManager;
        }

        public void Initialize(string userId, int? postId)
        {
            _state.ActiveUserId = userId;

            if (postId is not null)
            {
                using var scope = _serviceProvider.CreateScope();
                var getOneNews = scope.ServiceProvider.GetRequiredService<GetOneNews>();

                var singleNews = getOneNews.Do((int)postId);
                _state.DataModel = new NewsViewModel
                {
                    Id = singleNews.Id,
                    Title = singleNews.Title,
                    Body = singleNews.Body,
                    ImagePath = singleNews.Image,
                    Description = singleNews.Description,
                    Tags = singleNews.Tags,
                    Category = singleNews.Category
                };
            }
        }

        protected override async void HandleActions(IAction action)
        {
            switch (action.Name)
            {
                case PostNewsAction.PostNews:
                    await PostAction();
                    break;
                default:
                    break;
            }
        }

        public async Task PostAction()
        {
            using var scope = _serviceProvider.CreateScope();
            var createNews = scope.ServiceProvider.GetRequiredService<CreateNews>();
            var updateNews = scope.ServiceProvider.GetRequiredService<UpdateNews>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(_state.ActiveUserId);
            var name = user.UserName;

            if (_state.DataModel.Id > 0)
            {
                //Update post if it exists
                await updateNews.UpdateAsync(new UpdateNews.Request
                {
                    Id = _state.DataModel.Id,
                    Title = _state.DataModel.Title,
                    Body = _state.DataModel.Body,
                    ImageName = _state.DataModel.ImageName,
                    ImageStream = _state.DataModel.ImageStream,
                    Updated = DateTime.Now,
                    UpdatedBy = name,
                    Description = _state.DataModel.Description,
                    Tags = _state.DataModel.Tags,
                    Category = _state.DataModel.Category
                }); ;
            }
            else
            {
                //Create new post if not exists
                await createNews.CreateAsync(new CreateNews.Request
                {
                    Title = _state.DataModel.Title,
                    Body = _state.DataModel.Body,
                    ImageName = _state.DataModel.ImageName,
                    ImageStream = _state.DataModel.ImageStream,
                    Created = DateTime.Now,
                    CreatedBy = name,
                    Description = _state.DataModel.Description,
                    Tags = _state.DataModel.Tags,
                    Category = _state.DataModel.Category
                });
            }

            _navigationManager.NavigateTo($@"{_navigationManager.BaseUri}News/Manager/Index", true);
            //return RedirectToPage("/News/Manager/Index");
        }

    }
}
