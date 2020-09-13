using SWP.UI.Models;
using System;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp
{
    public abstract class BlazorAppBase
    {
        public string ActiveUserId { get; set; }
        public bool Loading { get; set; }
        public bool Initialized { get; set; }

        public UserModel User { get; set; } = new UserModel();

        public event EventHandler CallStateHasChanged;
        protected virtual void OnCallStateHasChanged(EventArgs e) => CallStateHasChanged?.Invoke(this, e);

        public abstract Task Initialize(string activeUserId);
    }
}
