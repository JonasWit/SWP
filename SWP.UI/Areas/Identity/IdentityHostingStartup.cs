using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(SWP.UI.Areas.Identity.IdentityHostingStartup))]
namespace SWP.UI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}