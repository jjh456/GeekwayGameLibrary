using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BoardGameLibrary.Startup))]
namespace BoardGameLibrary
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
