using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tehnika.Startup))]
namespace Tehnika
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
