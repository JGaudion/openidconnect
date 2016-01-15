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
        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<UserManager>();
            builder.RegisterType<RoleManager>();
            builder.RegisterType<UserStore>();
            builder.RegisterType<RoleStore>();
            //builder.RegisterType<AspNetUserStore>(resolver => new AspNetUserStore(ConnectionString));
        }
    }
}
