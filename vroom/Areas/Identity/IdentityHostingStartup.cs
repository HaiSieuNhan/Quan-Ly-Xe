using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(vroom.Areas.Identity.IdentityHostingStartup))]
namespace vroom.Areas.Identity
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