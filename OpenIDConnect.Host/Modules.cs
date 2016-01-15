using Autofac;
using OpenIDConnect.AdLds.Factories;
using OpenIDConnect.AdLds.Models;
using OpenIDConnect.AdLds.Services;
using OpenIDConnect.Core;
using OpenIDConnect.Core.Services;
using OpenIDConnect.IdentityServer.Factories;
using System;

namespace OpenIDConnect.Host
{
    public class Modules
    {
        public static void RegisterAll(ContainerBuilder builder)
        {
            RegisterHost(builder);
            RegisterAdLds(builder);
        }

        public static void RegisterHost(ContainerBuilder builder)
        {
            var configService = new ApplicationSettingsConfigurationService();
            builder.Register(ctx => configService).As<IConfigurationService>();
            UserStoreType userStoreType;
            Enum.TryParse(configService.GetSetting("IdentityServerUserStore", "None"), out userStoreType);
            builder.Register(ctx => userStoreType);
        }

        public static void RegisterAdLds(ContainerBuilder builder)
        {
            builder.RegisterType<AdLdsUserService>();
            builder.RegisterType<AdLdsDirectoryContextFactory>().As<IDirectoryContextFactory>();
            builder.Register(ctx => new DirectoryConnectionConfig("localhost", "389", "LDAP://", "CN=ADLDSUsers,DC=ScottLogic,DC=local"));
        }
    }
}