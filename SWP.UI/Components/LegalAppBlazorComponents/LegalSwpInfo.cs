using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalAppBlazorComponents
{
    public partial class LegalSwpInfo : IDisposable
    {

        public void Dispose()
        {

        }

        private bool[] toggles = new bool[8];


        private void ToggleSection(int sectionId)
        {
            for (int i = 0; i < toggles.Count(); i++)
            {
                toggles[i] = false;
            }

            toggles[sectionId] = true;

            StateHasChanged();
        }

    }
}
