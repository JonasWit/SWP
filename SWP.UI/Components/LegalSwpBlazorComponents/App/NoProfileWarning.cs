using SWP.UI.BlazorApp;
using System.Threading.Tasks;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class NoProfileWarning : BlazorPageBase
    {
        public LegalBlazorApp App { get; private set; }

        public NoProfileWarning()
        {
        }

        public override Task Initialize(BlazorAppBase app)
        {
            App = app as LegalBlazorApp;
            return Task.CompletedTask;
        }


    }
}
