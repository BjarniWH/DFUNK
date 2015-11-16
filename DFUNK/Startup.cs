using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DFUNK.Startup))]
namespace DFUNK
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
