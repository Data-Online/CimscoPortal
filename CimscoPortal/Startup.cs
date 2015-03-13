using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CimscoPortal.Startup))]
namespace CimscoPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
