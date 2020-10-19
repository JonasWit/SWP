using SWP.UI.BlazorApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents.App
{
    [UITransientService]
    public class DatabasePage : BlazorPageBase, IDisposable
    {
        public event EventHandler ActivePanelChanged;

        public class PanelChangedArgs : EventArgs
        {
            public Panels NewActivePanel { get; set; }
        }

        public AdminBlazorApp App { get; set; }

        public DatabasePage(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public Panels ActivePanel { get; private set; } = Panels.UsersStats;

        public void SetActivePanel(Panels panel)
        {
            ActivePanel = panel;
            ActivePanelChanged?.Invoke(this, new PanelChangedArgs { NewActivePanel = ActivePanel });
        } 

        public enum Panels
        { 
            LegalAppStats = 0,
            UsersStats = 1 
        }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as AdminBlazorApp;
            ActivePanelChanged += new EventHandler(ActivePanelHasChanged);
            return Task.CompletedTask;
        }

        private void ActivePanelHasChanged(object sender, EventArgs e)
        {
            var args = e as PanelChangedArgs;


        }

        public void Dispose()
        {
            ActivePanelChanged -= new EventHandler(ActivePanelHasChanged);
        }
    }
}
