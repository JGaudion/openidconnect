using Nancy;

namespace OpenIDConnect.Clients.AngularMaterial.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => View["index.html"];
            Get["/{uri*}"] = _ => View["index.html"];
        }
    }
}