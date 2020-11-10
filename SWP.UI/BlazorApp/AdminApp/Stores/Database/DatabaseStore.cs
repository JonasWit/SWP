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
        private readonly NotificationService _notificationService;

        public DatabaseState GetState() => _state;

        public DatabaseStore(
            IServiceProvider serviceProvider,
            NotificationService notificationService) : base(serviceProvider)
        {
            _state = new DatabaseState();
            _notificationService = notificationService;
        }

        public async Task InitializeState()
        {
            BroadcastStateChange();
        }

    }
}
