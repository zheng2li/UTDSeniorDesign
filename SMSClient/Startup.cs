using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SMSClient.Startup))]
namespace SMSClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
