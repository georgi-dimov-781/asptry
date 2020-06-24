using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MovieCatalog.Startup))]
namespace MovieCatalog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
