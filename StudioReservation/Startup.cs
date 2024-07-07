using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudioReservation.Startup))]
namespace StudioReservation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
