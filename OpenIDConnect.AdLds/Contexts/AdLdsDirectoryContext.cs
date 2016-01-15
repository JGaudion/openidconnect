using OpenIDConnect.AdLds.Extensions;
using OpenIDConnect.AdLds.Models;
using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Threading.Tasks;

namespace OpenIDConnect.AdLds.Contexts
{
    public class AdLdsDirectoryContext : IDirectoryContext
    {
        private readonly DirectoryConnectionConfig config;

        public AdLdsDirectoryContext(DirectoryConnectionConfig config)
        {
            this.config = config;
        }

        public Task<AdLdsUser> ValidateCredentialsAsync(string username, string password)
        {
            return Task.Run(() => ValidateCredentials(username, password));
        }

        public Task<AdLdsUser> FindUserByNameAsync(string username)
        {
            return Task.Run(() => FindUserByName(username));
        }

        private AdLdsUser FindUserByName(string username)
        {
            try
            {
                using (var principalContext = new PrincipalContext(ContextType.ApplicationDirectory, $"{this.config.ServerName}:{this.config.Port}", this.config.Container))
                {
                    var principal = UserPrincipal.FindByIdentity(principalContext, $"cn={username},{this.config.Container}");

                    if (principal != null)
                    {
                        return new AdLdsUser(principal.Name, principal.Name, principal.EmailAddress);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Info: Could not authenticate user {username}, failed with exception {e}");
                throw;
            }

            return null;
        }

        private AdLdsUser ValidateCredentials(string username, string password)
        {
            try
            {
                using (var principalContext = new PrincipalContext(ContextType.ApplicationDirectory, $"{this.config.ServerName}:{this.config.Port}", this.config.Container))
                {
                    if (principalContext.ValidateCredentials($"cn={username},{this.config.Container}", password, ContextOptions.SimpleBind))
                    {
                        var principal = UserPrincipal.FindByIdentity(principalContext, $"cn={username},{this.config.Container}");

                        if (principal != null)
                        {
                            return new AdLdsUser(principal.Name, principal.Name, principal.EmailAddress);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Info: Could not authenticate user {username}, failed with exception {e}");
                throw;
            }

            return null;
        }
    }
}
