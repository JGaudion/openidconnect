using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;
using IdentityModel;
using IdentityServer3.Core;
using IdentityServer3.Core.Extensions;
using OpenIDConnect.Core.Models;
using OpenIDConnect.Core.Services;

namespace OpenIDConnect.IdentityServer.MembershipReboot
{
    public class MembershipRebootUserService<TAccount> : IUserAuthenticationService
        where TAccount : UserAccount
    {
        public string DisplayNameClaimType { get; set; }

        protected readonly UserAccountService<TAccount> userAccountService;

        public MembershipRebootUserService(UserAccountService<TAccount> userAccountService)
        {
            if (userAccountService == null) throw new ArgumentNullException("userAccountService");

            this.userAccountService = userAccountService;
        }

        public Task SignOutAsync(ClaimsPrincipal subject, string clientId)
        {
            return Task.FromResult(0);
        }

        public Task<IEnumerable<Claim>> GetProfileDataAsync(ProfileDataRequest request)
        {
            var subject = request.Subject;
            var requestedClaimTypes = request.RequestedClaimTypes;

            var acct = userAccountService.GetByID(subject.GetSubjectId().ToGuid());
            if (acct == null)
            {
                throw new ArgumentException("Invalid subject identifier");
            }

            var claims = GetClaimsFromAccount(acct);
            if (requestedClaimTypes != null && requestedClaimTypes.Any())
            {
                claims = claims.Where(x => requestedClaimTypes.Contains(x.Type));
            }

            return Task.FromResult(claims);
        }

        protected IEnumerable<Claim> GetClaimsFromAccount(TAccount account)
        {
            var claims = new List<Claim>{
                new Claim(Constants.ClaimTypes.Subject, GetSubjectForAccount(account)),
                new Claim(Constants.ClaimTypes.UpdatedAt, EpochTimeExtensions.ToEpochTime(account.LastUpdated).ToString(), ClaimValueTypes.Integer),
                new Claim("tenant", account.Tenant),
                new Claim(Constants.ClaimTypes.PreferredUserName, account.Username),
            };

            if (!String.IsNullOrWhiteSpace(account.Email))
            {
                claims.Add(new Claim(Constants.ClaimTypes.Email, account.Email));
                claims.Add(new Claim(Constants.ClaimTypes.EmailVerified, account.IsAccountVerified ? "true" : "false"));
            }

            if (!String.IsNullOrWhiteSpace(account.MobilePhoneNumber))
            {
                claims.Add(new Claim(Constants.ClaimTypes.PhoneNumber, account.MobilePhoneNumber));
                claims.Add(new Claim(Constants.ClaimTypes.PhoneNumberVerified, !String.IsNullOrWhiteSpace(account.MobilePhoneNumber) ? "true" : "false"));
            }

            claims.AddRange(account.Claims.Select(x => new Claim(x.Type, x.Value)));
            claims.AddRange(userAccountService.MapClaims(account));

            return claims;
        }

        protected string GetSubjectForAccount(TAccount account)
        {
            return account.ID.ToString("D");
        }

        protected string GetDisplayNameForAccount(Guid accountID)
        {
            var acct = userAccountService.GetByID(accountID);
            var claims = GetClaimsFromAccount(acct);

            string name = null;
            if (DisplayNameClaimType != null)
            {
                name = acct.Claims.Where(x => x.Type == DisplayNameClaimType).Select(x => x.Value).FirstOrDefault();
            }
            return name
                ?? acct.Claims.Where(x => x.Type == Constants.ClaimTypes.Name).Select(x => x.Value).FirstOrDefault()
                ?? acct.Claims.Where(x => x.Type == ClaimTypes.Name).Select(x => x.Value).FirstOrDefault()
                ?? acct.Username;
        }

        protected Task<IEnumerable<Claim>> GetClaimsForAuthenticateResultAsync(TAccount account)
        {
            return Task.FromResult(Enumerable.Empty<Claim>());
        }

        public async Task<AuthenticationResult> AuthenticateLocalAsync(string username, string password, SignInData signInData)
        {
            AuthenticationResult result = null;

            try
            {
                TAccount account;
                if (ValidateLocalCredentials(username, password, signInData, out account))
                {
                    result = await PostAuthenticateLocalAsync(account, signInData);
                    if (result == null)
                    {
                        var subject = GetSubjectForAccount(account);
                        var name = GetDisplayNameForAccount(account.ID);

                        var claims = await GetClaimsForAuthenticateResultAsync(account);
                        result = new AuthenticationResult(subject, name, claims);
                    }
                }
                else
                {
                    if (account != null)
                    {
                        if (!account.IsLoginAllowed)
                        {
                            result = new AuthenticationResult("Account is not allowed to login");
                        }
                        else if (account.IsAccountClosed)
                        {
                            result = new AuthenticationResult("Account is closed");
                        }
                    }
                }
            }
            catch (ValidationException ex)
            {
                result = new AuthenticationResult(ex.Message);
            }

            return result;
        }

        public Task<AuthenticationResult> PostAuthenticateAsync(SignInData signInData, AuthenticationResult authenticationResult)
        {
            return Task.FromResult(authenticationResult);
        }

        public Task<AuthenticationResult> PreAuthenticateAsync(SignInData signInData)
        {
            return Task.FromResult<AuthenticationResult>(null);
        }

        protected Task<AuthenticationResult> PostAuthenticateLocalAsync(TAccount account, SignInData message)
        {
            return Task.FromResult<AuthenticationResult>(null);
        }

        protected bool ValidateLocalCredentials(string username, string password, SignInData message, out TAccount account)
        {
            var tenant = String.IsNullOrWhiteSpace(message.Tenant) ? userAccountService.Configuration.DefaultTenant : message.Tenant;
            return userAccountService.Authenticate(tenant, username, password, out account);
        }

        public async Task<AuthenticationResult> AuthenticateExternalAsync(ExternalIdentity identity, SignInData signInData)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("externalUser");
            }

            try
            {
                var tenant = String.IsNullOrWhiteSpace(signInData.Tenant) ? userAccountService.Configuration.DefaultTenant : signInData.Tenant;
                var acct = this.userAccountService.GetByLinkedAccount(tenant, identity.Provider, identity.ProviderId);
                if (acct == null)
                {
                    return await ProcessNewExternalAccountAsync(tenant, identity.Provider, identity.ProviderId, identity.Claims);
                }
                else
                {
                    return await ProcessExistingExternalAccountAsync(acct.ID, identity.Provider, identity.ProviderId, identity.Claims);
                }
            }
            catch (ValidationException ex)
            {
                return await Task.FromResult(new AuthenticationResult(ex.Message));
            }
        }

        protected async Task<AuthenticationResult> ProcessNewExternalAccountAsync(string tenant, string provider, string providerId, IEnumerable<Claim> claims)
        {
            var user = await TryGetExistingUserFromExternalProviderClaimsAsync(provider, claims);
            if (user == null)
            {
                user = await InstantiateNewAccountFromExternalProviderAsync(provider, providerId, claims);

                var email = claims.GetValue(Constants.ClaimTypes.Email);

                user = userAccountService.CreateAccount(
                    tenant,
                    Guid.NewGuid().ToString("N"),
                    null, email,
                    null, null,
                    user);
            }

            userAccountService.AddOrUpdateLinkedAccount(user, provider, providerId);

            var result = await AccountCreatedFromExternalProviderAsync(user.ID, provider, providerId, claims);
            if (result != null)
            {
                return result;
            }

            return await SignInFromExternalProviderAsync(user.ID, provider);
        }

        protected Task<TAccount> TryGetExistingUserFromExternalProviderClaimsAsync(string provider, IEnumerable<Claim> claims)
        {
            return Task.FromResult<TAccount>(null);
        }

        protected Task<TAccount> InstantiateNewAccountFromExternalProviderAsync(string provider, string providerId, IEnumerable<Claim> claims)
        {
            // we'll let the default creation happen, but can override to initialize properties if needed
            return Task.FromResult<TAccount>(null);
        }

        protected async Task<AuthenticationResult> AccountCreatedFromExternalProviderAsync(Guid accountID, string provider, string providerId, IEnumerable<Claim> claims)
        {
            SetAccountEmail(accountID, ref claims);
            SetAccountPhone(accountID, ref claims);

            return await UpdateAccountFromExternalClaimsAsync(accountID, provider, providerId, claims);
        }

        protected async Task<AuthenticationResult> SignInFromExternalProviderAsync(Guid accountID, string provider)
        {
            var account = userAccountService.GetByID(accountID);
            var claims = await GetClaimsForAuthenticateResultAsync(account);

            return new AuthenticationResult(
                subject: accountID.ToString("D"),
                name: GetDisplayNameForAccount(accountID),
                claims: claims,
                identityProvider: provider,
                authenticationMethod: Constants.AuthenticationMethods.External);
        }

        protected Task<AuthenticationResult> UpdateAccountFromExternalClaimsAsync(Guid accountID, string provider, string providerId, IEnumerable<Claim> claims)
        {
            userAccountService.AddClaims(accountID, new UserClaimCollection(claims));
            return Task.FromResult<AuthenticationResult>(null);
        }

        protected async Task<AuthenticationResult> ProcessExistingExternalAccountAsync(Guid accountID, string provider, string providerId, IEnumerable<Claim> claims)
        {
            return await SignInFromExternalProviderAsync(accountID, provider);
        }

        protected void SetAccountEmail(Guid accountID, ref IEnumerable<Claim> claims)
        {
            var email = claims.GetValue(Constants.ClaimTypes.Email);
            if (email != null)
            {
                var acct = userAccountService.GetByID(accountID);
                if (acct.Email == null)
                {
                    try
                    {
                        var email_verified = claims.GetValue(Constants.ClaimTypes.EmailVerified);
                        if (email_verified != null && email_verified == "true")
                        {
                            userAccountService.SetConfirmedEmail(acct.ID, email);
                        }
                        else
                        {
                            userAccountService.ChangeEmailRequest(acct.ID, email);
                        }

                        var emailClaims = new string[] { Constants.ClaimTypes.Email, Constants.ClaimTypes.EmailVerified };
                        claims = claims.Where(x => !emailClaims.Contains(x.Type));
                    }
                    catch (ValidationException)
                    {
                        // presumably the email is already associated with another account
                        // so eat the validation exception and let the claim pass thru
                    }
                }
            }
        }

        protected void SetAccountPhone(Guid accountID, ref IEnumerable<Claim> claims)
        {
            var phone = claims.GetValue(Constants.ClaimTypes.PhoneNumber);
            if (phone != null)
            {
                var acct = userAccountService.GetByID(accountID);
                if (acct.MobilePhoneNumber == null)
                {
                    try
                    {
                        var phone_verified = claims.GetValue(Constants.ClaimTypes.PhoneNumberVerified);
                        if (phone_verified != null && phone_verified == "true")
                        {
                            userAccountService.SetConfirmedMobilePhone(acct.ID, phone);
                        }
                        else
                        {
                            userAccountService.ChangeMobilePhoneRequest(acct.ID, phone);
                        }

                        var phoneClaims = new string[] { Constants.ClaimTypes.PhoneNumber, Constants.ClaimTypes.PhoneNumberVerified };
                        claims = claims.Where(x => !phoneClaims.Contains(x.Type));
                    }
                    catch (ValidationException)
                    {
                        // presumably the phone is already associated with another account
                        // so eat the validation exception and let the claim pass thru
                    }
                }
            }
        }

        public Task<bool> IsActiveAsync(ClaimsPrincipal subject, Client client)
        {
            var acct = userAccountService.GetByID(subject.GetSubjectId().ToGuid());

            var isActive = acct != null && !acct.IsAccountClosed && acct.IsLoginAllowed;

            return Task.FromResult(isActive);
        }
    }

    internal static class Extensions
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
    }
}
