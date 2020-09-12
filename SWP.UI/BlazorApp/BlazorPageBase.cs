using System.Threading.Tasks;

namespace SWP.UI.BlazorApp
{
    public abstract class BlazorPageBase
    {
        public abstract Task Initialize(BlazorAppBase app);

    }
}
