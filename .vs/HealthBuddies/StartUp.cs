using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HealthBuddies.App_Start.StartUp))]
namespace HealthBuddies.App_Start
{
    public partial class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}