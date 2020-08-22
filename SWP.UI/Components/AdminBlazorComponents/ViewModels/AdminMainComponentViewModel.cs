using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents.ViewModels
{
    public class AdminMainComponentViewModel
    {
        public enum Panels
        {
            Users = 0,
            Licenses = 1,
        }

        public Panels ActivePanel { get; set; } = Panels.Users;

        public void SetActivePanel(Panels panel) => ActivePanel = panel;

        #region NavBar

        public bool collapseNavMenu = true;

        public string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        public void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        #endregion

    }
}
