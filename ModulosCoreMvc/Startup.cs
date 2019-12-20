using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Modulos_Core_MVC.Startup))]
namespace Modulos_Core_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
