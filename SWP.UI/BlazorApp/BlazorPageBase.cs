using System;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp
{
    public abstract class BlazorPageBase : BlazorPageCore
    {
        public abstract Task Initialize(BlazorAppBase app);

        public BlazorPageBase(IServiceProvider serviceProvider) : base (serviceProvider) { }
    }
}
