using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Jitter.Startup))]
namespace Jitter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
