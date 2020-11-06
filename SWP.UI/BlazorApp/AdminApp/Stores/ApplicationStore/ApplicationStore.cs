using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.ApplicationStore
{
    public class ApplicationState
    {
        public bool Loading { get; set; } = false;

    }

    public class ApplicationStore : StoreBase
    {
        public ApplicationStore(IActionDispatcher actionDispatcher, IServiceProvider serviceProvider) : base(actionDispatcher, serviceProvider)
        {

        }

        protected override void HandleActions(IAction action)
        {
            throw new NotImplementedException();
        }
    }
}
