using OpenIDConnect.Core.Models;
using OpenIDConnect.Core.Services;
using OpenIDConnect.IdentityServer.AspNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenIDConnect.IdentityServer.AspNet.Services
{
    public class AspNetUserAuthenticationService : IUserAuthenticationService      
    {
        
        public string DisplayNameClaimType { get; set; }
        public bool EnableSecurityStamp { get; set; }

        protected readonly UserManager manager;

        /// <summary>
        /// This constructor will conjure up a UserManager as the Autofac will inject the dependencies
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="parseSubject"></param>
        public AspNetUserAuthenticationService(UserManager manager)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            this.manager = manager;
                        
            EnableSecurityStamp = true;
        }

        /// <summary>
        /// External: The user has logged in on some other site
        /// Either we are supplied a new identity, in which case we add it to our user store
        /// Or we are supplied an existing identity, in which case we return the claims for this user
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="signInData"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> AuthenticateExternalAsync(ExternalIdentity identity, SignInData signInData)
        {
            //There must be an identity for this to make sense
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            //The user manager tries to find a matching user
            var user = await manager.FindAsync(new Microsoft.AspNet.Identity.UserLoginInfo(identity.Provider, identity.ProviderId));
            if (user == null)
            {
                //Create a new account
                return await ProcessNewExternalAccountAsync(identity.Provider, identity.ProviderId, identity.Claims);
            }
            else
            {
                return await ProcessExistingExternalAccountAsync(user.Id, identity.Provider, identity.ProviderId, identity.Claims);
            }
        }

        /// <summary>
        /// Authentication from within our control, so we have the username and password.
        /// Find the user and get the claims. It doesn't seem to automatically create new users here.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="signInData"></param>
        /// <returns></returns>
        public async Task<AuthenticationResult> AuthenticateLocalAsync(string username, string password, SignInData signInData)
        {
            
            if (manager.SupportsUserPassword)
            {
                var user = await manager.FindByNameAsync(username);
                                   
                    if (user != null)
                    {
                        if (manager.SupportsUserLockout &&
                            await manager.IsLockedOutAsync(user.Id))
                        {
                            return null;
                        }

                        if (await manager.CheckPasswordAsync(user, password))
                        {
                            if (manager.SupportsUserLockout)
                            {
                                await manager.ResetAccessFailedCountAsync(user.Id);
                            }
                            var result = await PostAuthenticateLocalAsync(user, signInData);
                            if (result == null)
                            {
                               var claims = await GetClaimsForAuthenticateResult(user);
                               return new AuthenticationResult(user.Id.ToString(), await GetDisplayNameForAccountAsync(user.Id), claims);
                            }

                        return result; 
                        }
                        else if (manager.SupportsUserLockout)
                        {
                            await manager.AccessFailedAsync(user.Id);
                        }
                    }
                
            }

            return null;
        }

        /// <summary>
        /// Doing nothing
        /// </summary>
        /// <param name="user"></param>
        /// <param name="signInData"></param>
        /// <returns></returns>
        protected virtual Task<AuthenticationResult> PostAuthenticateLocalAsync(User user, SignInData signInData)
        {
            return Task.FromResult<AuthenticationResult>(null);
        }

        /// <summary>
        /// Go and get all claims for this user. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Claim>> GetProfileDataAsync(ProfileDataRequest request)
        {
            var subject = request.Subject;
            var requestedClaimTypes = request.RequestedClaimTypes;

            if (subject == null) throw new ArgumentNullException("subject");

            var acct = await manager.FindByIdAsync(subject.FindFirst(Core.Constants.ClaimTypes.Subject).Value);
            if (acct == null)
            {
                throw new ArgumentException("Invalid subject identifier");
            }

            var claims = await GetClaimsFromAccount(acct);
            if (requestedClaimTypes != null && requestedClaimTypes.Any())
            {
                claims = claims.Where(x => requestedClaimTypes.Contains(x.Type));
            }

            return claims;
        }

        /// <summary>
        /// Check if a specific user exists
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task<bool> IsActiveAsync(ClaimsPrincipal subject, Client client)
        {
           
            if (subject == null) throw new ArgumentNullException("subject");

            //Not sure about this
            var activeUser = await manager.FindByNameAsync(subject.Identity.Name);

            bool IsActive = false;

            if (activeUser != null)
            {
                if (EnableSecurityStamp && manager.SupportsUserSecurityStamp)
                {
                    var security_stamp = subject.Claims.Where(x => x.Type == "security_stamp").Select(x => x.Value).SingleOrDefault();
                    if (security_stamp != null)
                    {
                        var db_security_stamp = await manager.GetSecurityStampAsync(activeUser.Id);
                        if (db_security_stamp != security_stamp)
                        {
                            return false; //what do I do here, true or false or error?
                        }
                    }
                }

                IsActive = true;
            }
            return IsActive;
        }

        /// <summary>
        /// I'm not using this to do anything
        /// </summary>
        /// <param name="signInData"></param>
        /// <returns></returns>
        public Task<AuthenticationResult> PostAuthenticateAsync(SignInData signInData, AuthenticationResult authenticationResult)
        {
            return Task.FromResult<AuthenticationResult>(authenticationResult);
        }

        /// <summary>
        /// I've not found anywhere that actually uses this
        /// </summary>
        /// <param name="signInData"></param>
        /// <returns></returns>
        public Task<AuthenticationResult> PreAuthenticateAsync(SignInData signInData)
        {
            return Task.FromResult<AuthenticationResult>(null);
        }

        /// <summary>
        /// This currently doesn't exist. As far as I can tell once the user has the authentication cookie stored
        /// on their machine then the only way to sign them out is to refresh the cookie / clear it
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Task SignOutAsync(ClaimsPrincipal subject, string clientId)
        {
            return Task.FromResult(0);
        }

        #region SupportingMethods
        protected virtual async Task<IEnumerable<Claim>> GetClaimsForAuthenticateResult(User user)
        {
            List<Claim> claims = new List<Claim>();
            if (EnableSecurityStamp && manager.SupportsUserSecurityStamp)
            {
                var stamp = await manager.GetSecurityStampAsync(user.Id);
                if (!String.IsNullOrWhiteSpace(stamp))
                {
                    claims.Add(new Claim("security_stamp", stamp));
                }
            }
            return claims;
        }

        protected virtual async Task<string> GetDisplayNameForAccountAsync(string userID)
        {
            var user = await manager.FindByIdAsync(userID);
            var claims = await GetClaimsFromAccount(user);

            Claim nameClaim = null;
            if (DisplayNameClaimType != null)
            {
                nameClaim = claims.FirstOrDefault(x => x.Type == DisplayNameClaimType);
            }
            if (nameClaim == null) nameClaim = claims.FirstOrDefault(x => x.Type == OpenIDConnect.Core.Constants.ClaimTypes.Name);
            if (nameClaim == null) nameClaim = claims.FirstOrDefault(x => x.Type == OpenIDConnect.Core.Constants.ClaimTypes.Name);
            if (nameClaim != null) return nameClaim.Value;

            return user.UserName;
        }

        protected virtual async Task<IEnumerable<Claim>> GetClaimsFromAccount(User user)
        {
            var claims = new List<Claim>{
                new Claim(OpenIDConnect.Core.Constants.ClaimTypes.Subject, user.Id.ToString()),
                new Claim(OpenIDConnect.Core.Constants.ClaimTypes.PreferredUserName, user.UserName),
            };

            if (manager.SupportsUserEmail)
            {
                var email = await manager.GetEmailAsync(user.Id);
                if (!String.IsNullOrWhiteSpace(email))
                {
                    claims.Add(new Claim(OpenIDConnect.Core.Constants.ClaimTypes.Email, email));
                    var verified = await manager.IsEmailConfirmedAsync(user.Id);
                    claims.Add(new Claim(OpenIDConnect.Core.Constants.ClaimTypes.EmailVerified, verified ? "true" : "false"));
                }
            }

            if (manager.SupportsUserPhoneNumber)
            {
                var phone = await manager.GetPhoneNumberAsync(user.Id);
                if (!String.IsNullOrWhiteSpace(phone))
                {
                    claims.Add(new Claim(OpenIDConnect.Core.Constants.ClaimTypes.PhoneNumber, phone));
                    var verified = await manager.IsPhoneNumberConfirmedAsync(user.Id);
                    claims.Add(new Claim(OpenIDConnect.Core.Constants.ClaimTypes.PhoneNumberVerified, verified ? "true" : "false"));
                }
            }

            if (manager.SupportsUserClaim)
            {
                claims.AddRange(await manager.GetClaimsAsync(user.Id));
            }

            if (manager.SupportsUserRole)
            {
                var roleClaims =
                    from role in await manager.GetRolesAsync(user.Id)
                    select new Claim(OpenIDConnect.Core.Constants.ClaimTypes.Role, role);
                claims.AddRange(roleClaims);
            }

            return claims;
        }

        protected virtual async Task<AuthenticationResult> ProcessNewExternalAccountAsync(string provider, string providerId, IEnumerable<Claim> claims)
        {
            var user = await TryGetExistingUserFromExternalProviderClaimsAsync(provider, claims);
            if (user == null)
            {
                user = await InstantiateNewUserFromExternalProviderAsync(provider, providerId, claims);
                if (user == null)
                    throw new InvalidOperationException("CreateNewAccountFromExternalProvider returned null");

                var createResult = await manager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    return new AuthenticationResult(createResult.Errors.First());
                    
                }
            }

            var externalLogin = new Microsoft.AspNet.Identity.UserLoginInfo(provider, providerId);
            var addExternalResult = await manager.AddLoginAsync(user.Id, externalLogin);
            if (!addExternalResult.Succeeded)
            {
                return new AuthenticationResult(addExternalResult.Errors.First());
            }

            var result = await AccountCreatedFromExternalProviderAsync(user.Id, provider, providerId, claims);
            if (result != null) return result;

            return await SignInFromExternalProviderAsync(user.Id, provider);
        }

        protected virtual Task<User> InstantiateNewUserFromExternalProviderAsync(string provider, string providerId, IEnumerable<Claim> claims)
        {
            var user = new User() { UserName = Guid.NewGuid().ToString("N") };
            return Task.FromResult(user);
        }

        protected virtual Task<User> TryGetExistingUserFromExternalProviderClaimsAsync(string provider, IEnumerable<Claim> claims)
        {
            return Task.FromResult<User>(null);
        }

        protected virtual async Task<AuthenticationResult> AccountCreatedFromExternalProviderAsync(string userID, string provider, string providerId, IEnumerable<Claim> claims)
        {
            claims = await manager.SetAccountEmailAsync(userID, claims);
            claims = await manager.SetAccountPhoneAsync(userID, claims);

            return await UpdateAccountFromExternalClaimsAsync(userID, provider, providerId, claims);
        }

        protected virtual async Task<AuthenticationResult> SignInFromExternalProviderAsync(string userID, string provider)
        {
            var user = await manager.FindByIdAsync(userID);
            var claims = await GetClaimsForAuthenticateResult(user);

            return new AuthenticationResult(
                userID.ToString(),
                await GetDisplayNameForAccountAsync(userID),
                claims,
                authenticationMethod: AuthenticationMethods.External,
                identityProvider: provider);

        }

        protected virtual async Task<AuthenticationResult> UpdateAccountFromExternalClaimsAsync(string userID, string provider, string providerId, IEnumerable<Claim> claims)
        {
            var existingClaims = await manager.GetClaimsAsync(userID);
            var intersection = existingClaims.Intersect(claims);
            var newClaims = claims.Except(intersection);

            foreach (var claim in newClaims)
            {
                var result = await manager.AddClaimAsync(userID, claim);
                if (!result.Succeeded)
                {
                    return new AuthenticationResult(result.Errors.First());
                }
            }

            return null;
        }

        protected virtual async Task<AuthenticationResult> ProcessExistingExternalAccountAsync(string userID, string provider, string providerId, IEnumerable<Claim> claims)
        {
            return await SignInFromExternalProviderAsync(userID, provider);
        }
               
             

        #endregion
    }
}
