using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityManager.Configuration;
using OpenIDConnect.Host.AspNet.IdMgr;
using OpenIDConnect.Host.AspNet.IdSvr;

namespace OpenIDConnect.Host.AspNet.Services
{
    public class ASPManagerOptionsService
    {
        public IdentityManagerOptions GetManagerOptions()
        {
            var factory = new IdentityManagerServiceFactory();
            factory.ConfigureSimpleIdentityManagerService("SnakesDatabase"); //Name of my connection string in the config

            return new IdentityManagerOptions
            {
                Factory = factory
            };
        }
    }
}
