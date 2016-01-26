using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(OpenIDConnect.Clients.AngularMaterials.Startup))]

namespace OpenIDConnect.Clients.AngularMaterials
{
    /// <summary>
    /// This is an "Owin Startup Class" (ie. Add > New Items> Web> Owin Startup)
    /// </summary>
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Use nancy
            app.UseNancy();
                        
        }
    }
}
