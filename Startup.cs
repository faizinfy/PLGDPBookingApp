using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PLGDPBookingApp.Startup))]
namespace PLGDPBookingApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
