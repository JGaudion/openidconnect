using Nancy;

namespace OpenIDConnect.Clients.Angular14.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            this.Get["/"] = _ => this.View["index.html"];
        }
    }
}