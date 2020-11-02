using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.MainStore
{
    public class InitializeAction : IAction
    {
        public const string Initialize = "Initialize";
        public string Name => Initialize;
        public string UserId { get; set; }
    }
}
