using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Expense_Tracker.Startup))]
namespace Expense_Tracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
