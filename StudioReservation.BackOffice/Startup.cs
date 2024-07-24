using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudioReservation.BackOffice.Startup))]
namespace StudioReservation.BackOffice
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
