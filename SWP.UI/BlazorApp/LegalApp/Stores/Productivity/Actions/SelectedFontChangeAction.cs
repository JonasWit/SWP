using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Productivity.Actions
{
    public class SelectedFontChangeAction : IAction
    {
        public const string SelectedFontChange = "SELECTED_FONT_CHANGE";
        public string Name => SelectedFontChange;

        public object Arg { get; set; }
    }
}
