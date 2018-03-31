using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(First3.Startup))]
namespace First3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
