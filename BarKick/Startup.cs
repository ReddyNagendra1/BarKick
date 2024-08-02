using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BarKick.Startup))]
namespace BarKick
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
