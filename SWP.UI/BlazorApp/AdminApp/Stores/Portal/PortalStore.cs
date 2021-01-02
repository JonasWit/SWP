using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Database
{
    public class PortalState
    {



    }

    [UIScopedService]
    public class PortalStore : StoreBase<PortalState>
    {
        public PortalStore(IServiceProvider serviceProvider, IActionDispatcher actionDispatcher, NotificationService notificationService)
            : base(serviceProvider, actionDispatcher, notificationService) { }

        protected override void HandleActions(IAction action)
        {
            throw new NotImplementedException();
        }
    }
}