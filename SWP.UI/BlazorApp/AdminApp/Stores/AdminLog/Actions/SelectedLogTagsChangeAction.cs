using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.AdminLog.Actions
{
    public class SelectedLogTagsChangeAction : IAction
    {
        public const string SelectedLogTagsChangeChange = "SELECTED_LOG_TAGS_CHANGE";
        public string Name => SelectedLogTagsChangeChange;

        public object Arg { get; set; }
    }
}
