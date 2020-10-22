using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.UtilityComponents
{
    public partial class LoaderComponent
    {
        [Parameter]
        public string Message { get; set; }
        [Parameter]
        public EventCallback<string> MessageChanged { get; set; }
    }
}
