using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CheckersMVC.Startup))]
namespace CheckersMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
