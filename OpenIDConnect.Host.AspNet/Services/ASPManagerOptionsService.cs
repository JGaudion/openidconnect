using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityManager.Configuration;
using OpenIDConnect.Host.AspNet.IdMgr;
using OpenIDConnect.Host.AspNet.IdSvr;
using OpenIDConnect.Host.Interfaces;

namespace OpenIDConnect.Host.AspNet.Services
{
    public class ASPManagerOptionsService : IManagerOptionsService
    {
        public IdentityManagerOptions GetManagerOptions()
        {
            ///This is the link between the IdentityServer3 stuff and our identity management implementation 
            ///Apparently the whole point of this factory is to register dependencies
            var factory = new IdentityManagerServiceFactory();
            //This is registering the Identity Manager Service
            factory.ConfigureSimpleIdentityManagerService("NameMattersContext"); //Name of my connection string in the config

            return new IdentityManagerOptions
            {
                Factory = factory
            };
        }
    }
}
