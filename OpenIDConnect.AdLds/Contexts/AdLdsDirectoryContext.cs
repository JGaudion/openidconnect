using OpenIDConnect.AdLds.Models;
using OpenIDConnect.Core.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Claims;
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

        public Task<string> CreateUserAsync(string username, string password, IEnumerable<Claim> claims)
        {
            return Task.Run(() => CreateUser(username, password, claims));
        }

        public Task<QueryResult<AdLdsUser>> QueryUsersAsync(int start, int count)
        {
            return Task.Run(() => QueryUsers(start, count));
        }

        public Task<QueryResult<AdLdsRole>> QueryRolesAsync(int start, int count)
        {
            return Task.Run(() => QueryRoles(start, count));
        }

        private AdLdsUser FindUserByName(string username)
        {
            try
            {
                using (var principalContext = CreatePrincipalContext())
                {
                    var principal = UserPrincipal.FindByIdentity(principalContext, $"cn={username},{this.config.Container}");

                    if (principal != null)
                    {
                        return new AdLdsUser(principal.Name, principal.DisplayName, principal.UserPrincipalName);
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
                using (var principalContext = CreatePrincipalContext())
                {
                    if (principalContext.ValidateCredentials($"cn={username},{this.config.Container}", password, ContextOptions.SimpleBind))
                    {
                        var principal = UserPrincipal.FindByIdentity(principalContext, $"cn={username},{this.config.Container}");

                        if (principal != null)
                        {
                            return new AdLdsUser(principal.Name, principal.DisplayName, principal.UserPrincipalName);
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

        private string CreateUser(string username, string password, IEnumerable<Claim> claims)
        {
            var subjectId = Guid.NewGuid().ToString();

            if (string.IsNullOrEmpty(username))
            {
                username = subjectId;
            }

            try
            {
                using (var context = CreatePrincipalContext())
                using (var user = new UserPrincipal(context))
                {
                    user.Name = subjectId;
                    user.UserPrincipalName = username;
                    user.DisplayName = username;

                    var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

                    if (email != null)
                    {
                        user.EmailAddress = email.Value;
                    }

                    if (!string.IsNullOrEmpty(password))
                    {
                        user.SetPassword(password);
                    }

                    user.Save();

                    if (!string.IsNullOrEmpty(password))
                    {
                        user.Enabled = true;
                    }

                    user.Save();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while creating user {username}, exception: {e}");
                throw;
            }

            return subjectId;
        }

        private QueryResult<AdLdsUser> QueryUsers(int start, int count)
        {
            try
            {
                using (var principalContext = CreatePrincipalContext())
                {
                    using (var searcher = new PrincipalSearcher(new UserPrincipal(principalContext)))
                    {
                        var results = searcher.FindAll().ToList();
                        int total = results.Count;
                        var users = results.Skip(start).Take(count).Select(r =>
                        {
                            return new AdLdsUser(r.Name, r.DisplayName, r.UserPrincipalName);
                        }).ToList();
                        return new QueryResult<AdLdsUser>(start, count, total, null, users);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error returning users: {e}");
                throw;
            }
        }

        private QueryResult<AdLdsRole> QueryRoles(int start, int count)
        {
            try
            {
                using (var principalContext = CreatePrincipalContext())
                {
                    using (var searcher = new PrincipalSearcher(new GroupPrincipal(principalContext)))
                    {
                        var results = searcher.FindAll().ToList();
                        int total = results.Count;
                        var groups = results.Skip(start).Take(count).Select(r =>
                        {
                            return new AdLdsRole(r.Name, r.DisplayName, r.Description);
                        }).ToList();
                        return new QueryResult<AdLdsRole>(start, count, total, null, groups);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occurred querying roles: {e}");
                throw;
            }
        }

        private PrincipalContext CreatePrincipalContext()
        {
            return new PrincipalContext(ContextType.ApplicationDirectory, $"{this.config.ServerName}:{this.config.Port}", this.config.Container);
        }

    }
}
