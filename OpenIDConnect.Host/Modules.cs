using Autofac;
using OpenIDConnect.AdLds.Factories;
using OpenIDConnect.AdLds.Models;
using OpenIDConnect.AdLds.Services;
using OpenIDConnect.Core;
using OpenIDConnect.IdentityAdmin;
using OpenIDConnect.IdentityManager;
using OpenIDConnect.IdentityServer;
using OpenIDConnect.IdentityServer.AspNet.Model;
using OpenIDConnect.IdentityServer.AspNet.Services;
using OpenIDConnect.IdentityServer.Factories;
using System;

namespace OpenIDConnect.Host
{
    public class Modules
    {
        public static void Register(ContainerBuilder builder)
        {
            var configService = new ApplicationSettingsConfigurationService();
            builder.Register(ctx => configService).As<IConfigurationService>();

            RegisterUserStore(configService, builder);

            var identityServerUri = configService.GetSetting<string>("IdentityServerUri", null);
            var identityManagerUri = configService.GetSetting<string>("IdentityManagerUri", null);
            var identityAdminUri = configService.GetSetting<string>("IdentityAdminUri", null);

            builder.RegisterType<IdentityServerBootstrapper>();

            builder.RegisterType<IdentityAdminBootstrapper>()
                .WithParameter("identityServerUri", identityServerUri)
                .WithParameter("identityAdminUri", identityAdminUri);

            builder.RegisterType<IdentityManagerBootstrapper>()
                .WithParameter("identityServerUri", identityServerUri)
                .WithParameter("identityManagerUri", identityManagerUri);
        }

        private static void RegisterUserStore(IConfigurationService configService, ContainerBuilder builder)
        {
            UserStoreType userStoreType;
            Enum.TryParse(configService.GetSetting("IdentityServerUserStore", "None"), out userStoreType);
            builder.Register(ctx => userStoreType);

            switch (userStoreType)
            {
                case UserStoreType.AspNetIdentity:
                    RegisterAspNetIdentity(builder);
                    break;
                case UserStoreType.MembershipReboot:
                    RegisterMembershipReboot(builder);
                    break;
                case UserStoreType.AdLds:
                    RegisterAdLds(builder);
                    break;

                default:
                    throw new InvalidOperationException("Invalid user store type specified");
            }

        }

        private static void RegisterAdLds(ContainerBuilder builder)
        {
            builder.RegisterType<AdLdsUserService>();
            builder.RegisterType<AdLdsDirectoryContextFactory>().As<IDirectoryContextFactory>();
            builder.Register(ctx => new DirectoryConnectionConfig("localhost", "389", "LDAP://", "CN=ADLDSUsers,DC=ScottLogic,DC=local"));
        }

        /// <summary>
        /// This is a way of injecting dependencies, used iwth AutoFac. When the service is initialized it
        /// will come with the registered dependencies implemented. So, when the AspNetUserService is requested, even though
        /// it has a UserManager in the constructor, one will be conjured up. I think.
        /// </summary>
        /// <param name="builder"></param>
        private static void RegisterAspNetIdentity(ContainerBuilder builder)
        {
            builder.RegisterType<ConcreteAspNetUserService>();
            builder.RegisterType<UserManager>();
            builder.RegisterType<RoleManager>();
            builder.RegisterType<UserStore>();
            builder.RegisterType<RoleStore>();
            builder.RegisterType<AspNetUserStore>();
        }

        private static void RegisterMembershipReboot(ContainerBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}