using OpenIDConnect.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenIDConnect.Core.Models;
using System.Security.Claims;
using OpenIDConnect.IdentityServer.AspNet.Model;

namespace OpenIDConnect.IdentityServer.AspNet.Services
{
    public class AspNetUserService<TUser,TKey> : IUserAuthenticationService
        where TUser : class, Microsoft.AspNet.Identity.IUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        
        public string DisplayNameClaimType { get; set; }
        public bool EnableSecurityStamp { get; set; }

        protected readonly Microsoft.AspNet.Identity.UserManager<TUser, TKey> userManager;
        protected readonly Func<string, TKey> ConvertSubjectToKey;

        /// <summary>
        /// This constructor will conjure up a UserManager as the Autofac will inject the dependencies
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="parseSubject"></param>
        public AspNetUserService(Microsoft.AspNet.Identity.UserManager<TUser, TKey> manager, Func<string, TKey> parseSubject = null)
        {
            if (userManager == null) throw new ArgumentNullException("userManager");

            this.userManager = manager;

            if (parseSubject != null)
            {
                ConvertSubjectToKey = parseSubject;
            }
            else
            {
                //Set the function to translate the key
                var keyType = typeof(TKey);
                if (keyType == typeof(string)) ConvertSubjectToKey = subject => (TKey)ParseString(subject);
                else if (keyType == typeof(int)) ConvertSubjectToKey = subject => (TKey)ParseInt(subject);
                else if (keyType == typeof(uint)) ConvertSubjectToKey = subject => (TKey)ParseUInt32(subject);
                else if (keyType == typeof(long)) ConvertSubjectToKey = subject => (TKey)ParseLong(subject);
                else if (keyType == typeof(Guid)) ConvertSubjectToKey = subject => (TKey)ParseGuid(subject);
                else
                {
                    throw new InvalidOperationException("Key type not supported");
                }
            }

            EnableSecurityStamp = true;
        }

        #region KeyTransformation
        object ParseString(string sub)
        {
            return sub;
        }
        object ParseInt(string sub)
        {
            int key;
            if (!Int32.TryParse(sub, out key)) return 0;
            return key;
        }
        object ParseUInt32(string sub)
        {
            uint key;
            if (!UInt32.TryParse(sub, out key)) return 0;
            return key;
        }
        object ParseLong(string sub)
        {
            long key;
            if (!Int64.TryParse(sub, out key)) return 0;
            return key;
        }
        object ParseGuid(string sub)
        {
            Guid key;
            if (!Guid.TryParse(sub, out key)) return Guid.Empty;
            return key;
        }
        #endregion

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
            var user = await userManager.FindAsync(new Microsoft.AspNet.Identity.UserLoginInfo(identity.Provider, identity.ProviderId));
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
            AuthenticationResult r = new AuthenticationResult();
            if (userManager.SupportsUserPassword)
            {
                var user = await userManager.FindByNameAsync(username);
                                   
                    if (user != null)
                    {
                        if (userManager.SupportsUserLockout &&
                            await userManager.IsLockedOutAsync(user.Id))
                        {
                            return null;
                        }

                        if (await userManager.CheckPasswordAsync(user, password))
                        {
                            if (userManager.SupportsUserLockout)
                            {
                                await userManager.ResetAccessFailedCountAsync(user.Id);
                            }
                            var result = PostAuthenticateLocalAsync(user, signInData);
                            if (result == null)
                            {
                                var claims = await GetClaimsForAuthenticateResult(user);
                               // result = new AuthenticationResult(user.Id.ToString(), await GetDisplayNameForAccountAsync(user.Id), claims);
                            }

                            //What do I do here?
                            
                        }
                        else if (userManager.SupportsUserLockout)
                        {
                            await userManager.AccessFailedAsync(user.Id);
                        }
                    }
                
            }

            return r;
        }

        /// <summary>
        /// Doing nothing
        /// </summary>
        /// <param name="user"></param>
        /// <param name="signInData"></param>
        /// <returns></returns>
        protected virtual Task<AuthenticationResult> PostAuthenticateLocalAsync(TUser user, SignInData signInData)
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

            TKey key = ConvertSubjectToKey(subject.Identity.Name);//Not sure about this
            var acct = await userManager.FindByIdAsync(key);
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
            var activeUser = await userManager.FindByNameAsync(subject.Identity.Name);

            bool IsActive = false;

            if (activeUser != null)
            {
                if (EnableSecurityStamp && userManager.SupportsUserSecurityStamp)
                {
                    var security_stamp = subject.Claims.Where(x => x.Type == "security_stamp").Select(x => x.Value).SingleOrDefault();
                    if (security_stamp != null)
                    {
                        var db_security_stamp = await userManager.GetSecurityStampAsync(activeUser.Id);
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
        public Task<AuthenticationResult> PostAuthenticateAsync(SignInData signInData)
        {
            return Task.FromResult<AuthenticationResult>(null);
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
            throw new NotImplementedException();
        }

        #region SupportingMethods
        protected virtual async Task<IEnumerable<Claim>> GetClaimsForAuthenticateResult(TUser user)
        {
            List<Claim> claims = new List<Claim>();
            if (EnableSecurityStamp && userManager.SupportsUserSecurityStamp)
            {
                var stamp = await userManager.GetSecurityStampAsync(user.Id);
                if (!String.IsNullOrWhiteSpace(stamp))
                {
                    claims.Add(new Claim("security_stamp", stamp));
                }
            }
            return claims;
        }

        protected virtual async Task<string> GetDisplayNameForAccountAsync(TKey userID)
        {
            var user = await userManager.FindByIdAsync(userID);
            var claims = await GetClaimsFromAccount(user);

            Claim nameClaim = null;
            if (DisplayNameClaimType != null)
            {
                nameClaim = claims.FirstOrDefault(x => x.Type == DisplayNameClaimType);
            }
            if (nameClaim == null) nameClaim = claims.FirstOrDefault(x => x.Type == Claims.ClaimTypes.Name);
            if (nameClaim == null) nameClaim = claims.FirstOrDefault(x => x.Type == Claims.ClaimTypes.Name);
            if (nameClaim != null) return nameClaim.Value;

            return user.UserName;
        }

        protected virtual async Task<IEnumerable<Claim>> GetClaimsFromAccount(TUser user)
        {
            var claims = new List<Claim>{
                new Claim(Claims.ClaimTypes.Subject, user.Id.ToString()),
                new Claim(Claims.ClaimTypes.PreferredUserName, user.UserName),
            };

            if (userManager.SupportsUserEmail)
            {
                var email = await userManager.GetEmailAsync(user.Id);
                if (!String.IsNullOrWhiteSpace(email))
                {
                    claims.Add(new Claim(Claims.ClaimTypes.Email, email));
                    var verified = await userManager.IsEmailConfirmedAsync(user.Id);
                    claims.Add(new Claim(Claims.ClaimTypes.EmailVerified, verified ? "true" : "false"));
                }
            }

            if (userManager.SupportsUserPhoneNumber)
            {
                var phone = await userManager.GetPhoneNumberAsync(user.Id);
                if (!String.IsNullOrWhiteSpace(phone))
                {
                    claims.Add(new Claim(Claims.ClaimTypes.PhoneNumber, phone));
                    var verified = await userManager.IsPhoneNumberConfirmedAsync(user.Id);
                    claims.Add(new Claim(Claims.ClaimTypes.PhoneNumberVerified, verified ? "true" : "false"));
                }
            }

            if (userManager.SupportsUserClaim)
            {
                claims.AddRange(await userManager.GetClaimsAsync(user.Id));
            }

            if (userManager.SupportsUserRole)
            {
                var roleClaims =
                    from role in await userManager.GetRolesAsync(user.Id)
                    select new Claim(Claims.ClaimTypes.Role, role);
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

                var createResult = await userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    //  return new AuthenticationResult(createResult.Errors.First());
                    return new AuthenticationResult();
                }
            }

            var externalLogin = new Microsoft.AspNet.Identity.UserLoginInfo(provider, providerId);
            var addExternalResult = await userManager.AddLoginAsync(user.Id, externalLogin);
            if (!addExternalResult.Succeeded)
            {
                // return new AuthenticationResult(addExternalResult.Errors.First());
                return new AuthenticationResult();
            }

            var result = await AccountCreatedFromExternalProviderAsync(user.Id, provider, providerId, claims);
            if (result != null) return result;

            return await SignInFromExternalProviderAsync(user.Id, provider);
        }

        protected virtual Task<TUser> InstantiateNewUserFromExternalProviderAsync(string provider, string providerId, IEnumerable<Claim> claims)
        {
            var user = new TUser() { UserName = Guid.NewGuid().ToString("N") };
            return Task.FromResult(user);
        }

        protected virtual Task<TUser> TryGetExistingUserFromExternalProviderClaimsAsync(string provider, IEnumerable<Claim> claims)
        {
            return Task.FromResult<TUser>(null);
        }

        protected virtual async Task<AuthenticationResult> AccountCreatedFromExternalProviderAsync(TKey userID, string provider, string providerId, IEnumerable<Claim> claims)
        {
            claims = await SetAccountEmailAsync(userID, claims);
            claims = await SetAccountPhoneAsync(userID, claims);

            return await UpdateAccountFromExternalClaimsAsync(userID, provider, providerId, claims);
        }

        protected virtual async Task<AuthenticationResult> SignInFromExternalProviderAsync(TKey userID, string provider)
        {
            var user = await userManager.FindByIdAsync(userID);
            var claims = await GetClaimsForAuthenticateResult(user);

            //return new AuthenticationResult(
            //    userID.ToString(),
            //    await GetDisplayNameForAccountAsync(userID),
            //    claims,
            //    authenticationMethod: AuthenticationMethods.External,
            //    identityProvider: provider);
            return new AuthenticationResult();
        }

        protected virtual async Task<AuthenticationResult> UpdateAccountFromExternalClaimsAsync(TKey userID, string provider, string providerId, IEnumerable<Claim> claims)
        {
            var existingClaims = await userManager.GetClaimsAsync(userID);
            var intersection = existingClaims.Intersect(claims);
            var newClaims = claims.Except(intersection);

            foreach (var claim in newClaims)
            {
                var result = await userManager.AddClaimAsync(userID, claim);
                if (!result.Succeeded)
                {
                    //return new AuthenticationResult(result.Errors.First());
                    return new AuthenticationResult();
                }
            }

            return null;
        }

        protected virtual async Task<AuthenticationResult> ProcessExistingExternalAccountAsync(TKey userID, string provider, string providerId, IEnumerable<Claim> claims)
        {
            return await SignInFromExternalProviderAsync(userID, provider);
        }

        protected virtual async Task<IEnumerable<Claim>> SetAccountEmailAsync(TKey userID, IEnumerable<Claim> claims)
        {
            var email = claims.FirstOrDefault(x => x.Type == Claims.ClaimTypes.Email);
            if (email != null)
            {
                var userEmail = await userManager.GetEmailAsync(userID);
                if (userEmail == null)
                {
                    // if this fails, then presumably the email is already associated with another account
                    // so ignore the error and let the claim pass thru
                    var result = await userManager.SetEmailAsync(userID, email.Value);
                    if (result.Succeeded)
                    {
                        var email_verified = claims.FirstOrDefault(x => x.Type == Claims.ClaimTypes.EmailVerified);
                        if (email_verified != null && email_verified.Value == "true")
                        {
                            var token = await userManager.GenerateEmailConfirmationTokenAsync(userID);
                            await userManager.ConfirmEmailAsync(userID, token);
                        }

                        var emailClaims = new string[] { Claims.ClaimTypes.Email, Claims.ClaimTypes.EmailVerified };
                        return claims.Where(x => !emailClaims.Contains(x.Type));
                    }
                }
            }

            return claims;
        }

        protected virtual async Task<IEnumerable<Claim>> SetAccountPhoneAsync(TKey userID, IEnumerable<Claim> claims)
        {
            var phone = claims.FirstOrDefault(x => x.Type == Claims.ClaimTypes.PhoneNumber);
            if (phone != null)
            {
                var userPhone = await userManager.GetPhoneNumberAsync(userID);
                if (userPhone == null)
                {
                    // if this fails, then presumably the phone is already associated with another account
                    // so ignore the error and let the claim pass thru
                    var result = await userManager.SetPhoneNumberAsync(userID, phone.Value);
                    if (result.Succeeded)
                    {
                        var phone_verified = claims.FirstOrDefault(x => x.Type == Claims.ClaimTypes.PhoneNumberVerified);
                        if (phone_verified != null && phone_verified.Value == "true")
                        {
                            var token = await userManager.GenerateChangePhoneNumberTokenAsync(userID, phone.Value);
                            await userManager.ChangePhoneNumberAsync(userID, phone.Value, token);
                        }

                        var phoneClaims = new string[] { Claims.ClaimTypes.PhoneNumber, Claims.ClaimTypes.PhoneNumberVerified };
                        return claims.Where(x => !phoneClaims.Contains(x.Type));
                    }
                }
            }

            return claims;
        }

        #endregion
    }
}
