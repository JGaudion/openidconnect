using Autofac;
using Autofac.Core;
using OpenIDConnect.IdentityServer.AspNet.Model;
using OpenIDConnect.IdentityServer.AspNet.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIDConnect.IdentityServer.AspNet
{
    public class Module
    {
        /// <summary>
        /// This is a way of injecting dependencies, used iwth AutoFac. When the service is initialized it
        /// will come with the registered dependencies implemented. So, when the AspNetUserService is requested, even though
        /// it has a UserManager in the constructor, one will be conjured up. I think.
        /// </summary>
        /// <param name="builder"></param>
        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<UserManager>();
            builder.RegisterType<RoleManager>();
            builder.RegisterType<UserStore>();
            builder.RegisterType<RoleStore>();
            builder.RegisterType<AspNetUserStore>();
        }
    }
}
