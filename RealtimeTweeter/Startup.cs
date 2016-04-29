using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RealtimeTweeter.Startup))]
namespace RealtimeTweeter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();

            ConfigureAuth(app);
        }
    }
}
