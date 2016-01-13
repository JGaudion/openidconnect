using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIDConnect.Host.AspNet.IdSvr
{
    public class TemporaryScopes
    {
        public static IEnumerable<IdentityServer3.Core.Models.Scope> Get()
        {
            return new IdentityServer3.Core.Models.Scope[]
            {
                IdentityServer3.Core.Models.StandardScopes.OpenId,
                IdentityServer3.Core.Models.StandardScopes.Profile,
                IdentityServer3.Core.Models.StandardScopes.Email,
                IdentityServer3.Core.Models.StandardScopes.OfflineAccess,
                new IdentityServer3.Core.Models.Scope
                {
                    Name = "read",
                    DisplayName = "Read data",
                    Type = IdentityServer3.Core.Models.ScopeType.Resource,
                    Emphasize = false,
                },
                new IdentityServer3.Core.Models.Scope
                {
                    Name = "write",
                    DisplayName = "Write data",
                    Type = IdentityServer3.Core.Models.ScopeType.Resource,
                    Emphasize = true,
                },
                new IdentityServer3.Core.Models.Scope
                {
                    Name = "forbidden",
                    DisplayName = "Forbidden scope",
                    Type = IdentityServer3.Core.Models.ScopeType.Resource,
                    Emphasize = true
                }
             };
        }
    }
}
