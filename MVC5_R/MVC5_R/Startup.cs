using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC5_R.Startup))]
namespace MVC5_R
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
