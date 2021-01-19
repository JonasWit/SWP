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
        public bool switchSection = false;

        public void SectionToggle() => switchSection = !switchSection;
    }
}
