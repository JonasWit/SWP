using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Database
{
    public class DatabaseState
    {



    }

    [UIScopedService]
    public class DatabaseStore : StoreBase<DatabaseState>
    {
        public DatabaseStore(
            IServiceProvider serviceProvider,
            IActionDispatcher actionDispatcher,
            NotificationService notificationService) 
            : base(serviceProvider, actionDispatcher, notificationService)
        {

        }

        protected override void HandleActions(IAction action)
        {
            throw new NotImplementedException();
        }

        public override void CleanUpStore()
        {
            throw new NotImplementedException();
        }

        public override void RefreshSore()
        {
            throw new NotImplementedException();
        }
    }
}
