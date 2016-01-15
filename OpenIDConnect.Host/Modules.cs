using Autofac;
using OpenIDConnect.AdLds.Factories;
using OpenIDConnect.AdLds.Models;
using OpenIDConnect.AdLds.Services;
using OpenIDConnect.Core.Services;

namespace OpenIDConnect.Host
{
    public class Modules
    {
        public static void RegisterAll(ContainerBuilder builder)
        {
            RegisterAdLds(builder);
        }

        public static void RegisterAdLds(ContainerBuilder builder)
        {
            builder.RegisterType<AdLdsUserService>().As<IUserAuthenticationService>();
            builder.RegisterType<AdLdsDirectoryContextFactory>().As<IDirectoryContextFactory>();
            builder.Register(ctx => new DirectoryConnectionConfig("localhost", "389", "LDAP://", "CN=ADLDSUsers,DC=ScottLogic,DC=local"));
        }
    }
}