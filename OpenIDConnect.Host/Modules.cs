﻿using Autofac;
using OpenIDConnect.AdLds.Factories;
using OpenIDConnect.AdLds.Models;
using OpenIDConnect.AdLds.Services;
using OpenIDConnect.Core;
using OpenIDConnect.Core.Services;
using OpenIDConnect.IdentityAdmin;
using OpenIDConnect.IdentityManager;
using OpenIDConnect.IdentityServer;
using OpenIDConnect.IdentityServer.AspNet.Model;
using OpenIDConnect.IdentityServer.AspNet.Services;
using OpenIDConnect.IdentityServer.Factories;
using System;
using BrockAllen.MembershipReboot;
using OpenIDConnect.IdentityServer.MembershipReboot;
using OpenIDConnect.IdentityServer.MembershipReboot.WrapperClasses;

namespace OpenIDConnect.Host
{
    public class Modules
    {
        public static void Register(ContainerBuilder builder)
        {
            var configService = new ApplicationSettingsConfigurationService();
            builder.Register(ctx => configService).As<IConfigurationService>();

            RegisterUserStore(configService, builder);
            RegisterExternalIdentityProviders(configService, builder);

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
            builder.RegisterType<AdLdsUserAuthenticationService>().As<IUserAuthenticationService>();
            builder.RegisterType<AdLdsUserManagementService>().As<IUserManagementService>();
            builder.RegisterType<AdLdsDirectoryContextFactory>().As<IDirectoryContextFactory>();
            builder.RegisterType<DirectoryConnectionConfig>();
        }

        /// <summary>
        /// This is a way of injecting dependencies, used iwth AutoFac. When the service is initialized it
        /// will come with the registered dependencies implemented. So, when the AspNetUserService is requested, even though
        /// it has a UserManager in the constructor, one will be conjured up. I think.
        /// </summary>
        /// <param name="builder"></param>
        private static void RegisterAspNetIdentity(ContainerBuilder builder)
        {
            builder.RegisterType<ConcreteAspNetUserService>().As<IUserAuthenticationService>().ExternallyOwned();
            builder.RegisterType<UserManager>().ExternallyOwned();
            builder.RegisterType<RoleManager>().ExternallyOwned();
            builder.RegisterType<UserStore>().ExternallyOwned();
            builder.RegisterType<RoleStore>().ExternallyOwned();
            builder.RegisterType<AspNetUserStore>().ExternallyOwned();
        }

        private static void RegisterMembershipReboot(ContainerBuilder builder)
        {
            builder.RegisterType<MembershipRebootUserService<CustomUser>>().As<IUserAuthenticationService>().ExternallyOwned();
            builder.RegisterType<CustomUserAccountService>().As<UserAccountService<CustomUser>>().ExternallyOwned();
            builder.RegisterType<CustomUserRepository>().ExternallyOwned();

            builder.Register(resolver => new CustomDatabase("MembershipReboot")).ExternallyOwned();
            builder.Register(c => CustomConfig.Config).ExternallyOwned();
        }

        private static void RegisterExternalIdentityProviders(IConfigurationService configService, ContainerBuilder builder)
        {
            var externalIdentityProviderService = new ExternalIdentityProviderService()
                .WithGoogleAuthentication(configService.GetSetting("externalProviders:google:clientId", string.Empty), configService.GetSetting("externalProviders:google:clientSecret", string.Empty))
                .WithFacebookAuthentication(configService.GetSetting("externalProviders:facebook:clientId", string.Empty), configService.GetSetting("externalProviders:facebook:clientSecret", string.Empty))
                .WithTwitterAuthentication(configService.GetSetting("externalProviders:twitter:clientId", string.Empty), configService.GetSetting("externalProviders:twitter:clientSecret", string.Empty));

            builder.Register(ctx => externalIdentityProviderService);
        }
    }
}