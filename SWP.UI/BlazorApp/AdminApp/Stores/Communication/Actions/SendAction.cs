using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Communication.Actions
{
    public class SendAction : IAction
    {
        public const string Send = "SEND";
        public string Name => Send;
    }
}
