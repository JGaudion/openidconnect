using IdentityManager;
using IdentityManager.Configuration;
using Microsoft.AspNet.Identity.EntityFramework;
using OpenIDConnect.Host.AspNet.EntitiesAndStores;
using OpenIDConnect.Host.AspNet.IdMgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIDConnect.Host.AspNet.IdSvr
{
    public static class ExtensionMethods
    {
        public static void ConfigureSimpleIdentityManagerService(this IdentityManagerServiceFactory factory, string connectionString)
        {
            ///This is telling the IdentityManagerServiceFactory which dependencies it needs (using my implementations)
            factory.Register(new IdentityManager.Configuration.Registration<Context>(resolver => new Context(connectionString)));
            factory.Register(new Registration<UserStore>());
            factory.Register(new Registration<RoleStore>());
            factory.Register(new Registration<UserManager>());
            factory.Register(new Registration<RoleManager>());
            ///Register which identity manager service we are using. The second part (Simple Identity Manager Service) points to our
            ///actual implementiation. The IIdentityManagerService contains default async methods for things like adding a user
            ///or getting the user claims etc
            ///This style of registration means: the first thing is what is dependant, the second is what it is dependant on (the implementation to use)
            ///e.g. SimpleIdentityManagerService is the concrete version of IIdentityManagerService
            factory.IdentityManagerService = new Registration<IIdentityManagerService, SimpleIdentityManagerService>();
        }

        public static void ConfigureUserService(this IdentityServer3.Core.Configuration.IdentityServerServiceFactory factory, string connString)
        {
            factory.UserService = new IdentityServer3.Core.Configuration.Registration<IdentityServer3.Core.Services.IUserService, UserService>();
            factory.Register(new IdentityServer3.Core.Configuration.Registration<UserManager>());
            factory.Register(new IdentityServer3.Core.Configuration.Registration<UserStore>());
            factory.Register(new IdentityServer3.Core.Configuration.Registration<Context>(resolver => new Context(connString)));
        }
    }
}
