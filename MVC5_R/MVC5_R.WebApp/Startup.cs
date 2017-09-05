using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC5_R.WebApp.Startup))]
namespace MVC5_R.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
