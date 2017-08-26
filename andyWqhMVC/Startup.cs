using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(andyWqhMVC.Startup))]
namespace andyWqhMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
