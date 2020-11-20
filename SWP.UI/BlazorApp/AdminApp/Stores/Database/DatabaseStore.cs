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
    public class DatabaseStore : StoreBase
    {
        private readonly DatabaseState _state;

        public DatabaseState GetState() => _state;

        public DatabaseStore(
            IServiceProvider serviceProvider,
            IActionDispatcher actionDispatcher,
            NotificationService notificationService) 
            : base(serviceProvider, actionDispatcher, notificationService)
        {
            _state = new DatabaseState();
        }

        protected override void HandleActions(IAction action)
        {
            throw new NotImplementedException();
        }

        public override void CleanUpStore()
        {
            throw new NotImplementedException();
        }
    }
}
