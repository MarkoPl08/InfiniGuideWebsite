using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IG_Website.Startup))]
namespace IG_Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {            
            ConfigureAuth(app);
        }
    }
}
