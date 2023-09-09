using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MediX.Startup))]
namespace MediX
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
