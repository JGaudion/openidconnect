using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
using System.DirectoryServices;
using System;
using IdentityServer3.Core.Extensions;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer3.Core;

namespace OpenIDConnect.IdentityServer3.AdLds
{
    public class AdLdsUserService : UserServiceBase
    {
        private readonly DirectoryEntry directoryEntry;

        public AdLdsUserService()
        {
            string server = "localhost";
            string port = "389";
            string objectName = "CN=ADLDSUsers,DC=ScottLogic,DC=local";
            string path = "LDAP://";

            path = $"{path}{server}:{port}/{objectName}";

            try
            {
                directoryEntry = new DirectoryEntry(path);
                directoryEntry.RefreshCache();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: directory bind for path {path} failed. Exception: {e}");
                throw;
            }
        }

        public async override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var principal = context.Subject;
            var requestedClaimTypes = context.RequestedClaimTypes;

            var user = (await GetUserByName(principal.Identity.GetName())).ToAdLdsUser();

            var claims = GetClaimsForResult(user);
        }

        public async override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            var username = context.UserName;
            var password = context.Password;

            AuthenticateResult result = null;

            try
            {
                var validateContext = new ValidateLocalCredentialsContext(username, password);
                if (await ValidateLocalCredentials(validateContext))
                {
                    result = new AuthenticateResult(validateContext.User.Id, validateContext.User.Name);
                }
                else
                {
                    result = new AuthenticateResult("Account is not able to log in.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception occurred while authenticating user {username}: {e}");
                throw;
            }
        }

        public override Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {
            throw new NotImplementedException("AuthenticateExternalAsync not implemented");
        }

        public override async Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject;

            var account = await GetUserByName(subject.Identity.GetName());

            context.IsActive = account != null;
        }

        private async Task<bool> ValidateLocalCredentials(ValidateLocalCredentialsContext context)
        {
            var result = await GetUserByName(context.UserName);

            var passwordResult = result.Properties["userPassword"]?.ToString();

            if (context.Password == passwordResult)
            {
                context.User = result.ToAdLdsUser();
                return true;
            }

            return false;
        }

        private IEnumerable<Claim> GetClaimsForResult(AdLdsUser user)
        {
            yield return new Claim(Constants.ClaimTypes.Subject, user.Id);
            yield return new Claim(Constants.ClaimTypes.PreferredUserName, user.Name);
            
            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                yield return new Claim(Constants.ClaimTypes.Email, user.Email);
            }
        }

        private Task<SearchResult> GetUserByName(string name)
        {
            string query = $"(&(objectClass=user)(cn={name}))";

            SearchResultCollection searchResults;

            return Task.Run(() =>
            {
                try
                {
                    var searcher = new DirectorySearcher(directoryEntry);
                    searcher.Filter = query;
                    searcher.SearchScope = SearchScope.Subtree;
                    searchResults = searcher.FindAll();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: query \"{query}\" failed with exception {e}");
                    throw;
                }

                if (searchResults.Count > 1)
                {
                    throw new InvalidOperationException($"Too many results for query {query}");
                }

                return searchResults.Count > 0 ? searchResults[0] : null;
            });
        }
    }

    static class Extensions
    {
        public static Guid ToGuid(this string s)
        {
            Guid g;
            if (Guid.TryParse(s, out g))
            {
                return g;
            }

            return Guid.Empty;
        }

        public static AdLdsUser ToAdLdsUser(this SearchResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            var id = result.Properties["cn"]?.ToString();
            var name = result.Properties["Name"]?.ToString();
            var email = result.Properties["mail"]?.ToString();

            return new AdLdsUser(id, name, email);
        }
    }
}
