using OpenIDConnect.Host.AspNet.EntitiesAndStores;
using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIDConnect.Host.AspNet.IdSvr
{
    public class UserService : IdentityServer3.AspNetIdentity.AspNetIdentityUserService<User, string>
    {
        public UserService(UserManager userMgr)
            : base(userMgr)
        {
        }

        /// <summary>
        /// I *think* this is only if we add extra fields to the user in the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected override async Task<IEnumerable<System.Security.Claims.Claim>> GetClaimsFromAccount(User user)
        {
            var claims = (await base.GetClaimsFromAccount(user)).ToList();
            if (!String.IsNullOrWhiteSpace(user.FirstName))
            {
                claims.Add(new System.Security.Claims.Claim("given_name", user.FirstName));
            }
            if (!String.IsNullOrWhiteSpace(user.LastName))
            {
                claims.Add(new System.Security.Claims.Claim("family_name", user.LastName));
            }

            return claims;
        }
    }
}
