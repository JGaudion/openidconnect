using Microsoft.AspNet.Identity;
using OpenIDConnect.Core.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OpenIDConnect.IdentityServer.AspNet.Model
{
    public class UserManager : UserManager<User, string>
    {
        public UserManager(UserStore store) :base(store)
        {

        }
        /// <summary>
        /// Checks to see if this email has already been assigned to a user. If it has - ignore this claim. 
        /// Otherwise add the email to the user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        internal virtual async Task<IEnumerable<Claim>> SetAccountEmailAsync(string userID, IEnumerable<Claim> claims)
        {
            var email = claims.FirstOrDefault(x => x.Type == OpenIDConnect.Core.Constants.ClaimTypes.Email);
            if (email != null)
            {
                var userEmail = await this.GetEmailAsync(userID);
                if (userEmail == null)
                {
                    // if this fails, then presumably the email is already associated with another account
                    // so ignore the error and let the claim pass thru
                    var result = await this.SetEmailAsync(userID, email.Value);
                    if (result.Succeeded)
                    {
                        var email_verified = claims.FirstOrDefault(x => x.Type == OpenIDConnect.Core.Constants.ClaimTypes.EmailVerified);
                        if (email_verified != null && email_verified.Value == "true")
                        {
                            var token = await this.GenerateEmailConfirmationTokenAsync(userID);
                            await this.ConfirmEmailAsync(userID, token);
                        }

                        var emailClaims = new string[] { OpenIDConnect.Core.Constants.ClaimTypes.Email, OpenIDConnect.Core.Constants.ClaimTypes.EmailVerified };
                        return claims.Where(x => !emailClaims.Contains(x.Type));
                    }
                }
            }

            return claims;
        }

        /// <summary>
        /// Checks to see if this phone has already been assigned to a user. If it has - ignore this claim. 
        /// Otherwise add the phone to the user
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        internal virtual async Task<IEnumerable<Claim>> SetAccountPhoneAsync(string userID, IEnumerable<Claim> claims)
        {
            var phone = claims.FirstOrDefault(x => x.Type == OpenIDConnect.Core.Constants.ClaimTypes.PhoneNumber);
            if (phone != null)
            {
                var userPhone = await this.GetPhoneNumberAsync(userID);
                if (userPhone == null)
                {
                    // if this fails, then presumably the phone is already associated with another account
                    // so ignore the error and let the claim pass thru
                    var result = await this.SetPhoneNumberAsync(userID, phone.Value);
                    if (result.Succeeded)
                    {
                        var phone_verified = claims.FirstOrDefault(x => x.Type == OpenIDConnect.Core.Constants.ClaimTypes.PhoneNumberVerified);
                        if (phone_verified != null && phone_verified.Value == "true")
                        {
                            var token = await this.GenerateChangePhoneNumberTokenAsync(userID, phone.Value);
                            await this.ChangePhoneNumberAsync(userID, phone.Value, token);
                        }

                        var phoneClaims = new string[] { OpenIDConnect.Core.Constants.ClaimTypes.PhoneNumber, OpenIDConnect.Core.Constants.ClaimTypes.PhoneNumberVerified };
                        return claims.Where(x => !phoneClaims.Contains(x.Type));
                    }
                }
            }

            return claims;
        }

        /// <summary>
        /// Checks to see which roles the user already has, and then adds the any that they do not already have
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        internal virtual async Task<IEnumerable<Claim>> SetRolesForUser(string userID, IEnumerable<Claim> claims)
        {
            var roles = claims.Where(c => c.Type == OpenIDConnect.Core.Constants.ClaimTypes.Role);
            if (roles != null)
            {
                var existingRoles = await this.GetRolesAsync(userID);
                var newRoles = roles.Where(r => !existingRoles.Contains(r.Value));
                foreach (var role in newRoles)
                {
                    this.AddToRole(userID, role.Value);
                }
            }
            return claims;
        }

        /// <summary>
        /// Checks to see which claims the user already has, then adds any that it doesn't have
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        internal virtual async Task<IEnumerable<Claim>> SetOtherClaims (string userID, IEnumerable<Claim> claims)
        {
            var MiscClaims = claims.Where(c => c.Type != OpenIDConnect.Core.Constants.ClaimTypes.Role && c.Type != OpenIDConnect.Core.Constants.ClaimTypes.Email && c.Type != OpenIDConnect.Core.Constants.ClaimTypes.PhoneNumber);

            if(MiscClaims != null)
            {
                var existingClaims = await this.GetClaimsAsync(userID);
                var newClaims = claims.Where(c => !existingClaims.Any(x=> x.Value == c.Value));
                foreach(var claim in newClaims)
                {
                    this.AddClaim(userID, claim);
                }
            }
            return claims;
        }

        private QueryResult<User> QueryUsers(int start, int count, string filter)
        {
            var results = this.Users.ToList();
            List<User> selectedUsers = new List<User>();
            if (!string.IsNullOrEmpty(filter))
            {
                //Could use dynamic linq?
                selectedUsers = selectedUsers.Where(u => u.UserName.Contains(filter)).ToList();
            }

            int total = results.Count();
            selectedUsers = results.Skip(start).Take(count).ToList();
           
            return new QueryResult<User>(start, count, total, null, selectedUsers);
        }

        public Task<QueryResult<User>> QueryUsersAsync(int start, int count, string filter)
        {
            return Task.Run(() => QueryUsers(start, count, filter));
        }

        public QueryResult<TOut> ToDomain<TIn, TOut>(QueryResult<TIn> result, Func<TIn, TOut> resultConverter)
        {
            return new QueryResult<TOut>(result.Start, result.Count, result.Total, result.Filter, result.Items.Select(resultConverter));
        }

    }
}
