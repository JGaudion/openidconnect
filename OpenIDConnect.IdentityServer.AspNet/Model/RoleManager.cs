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
    public class RoleManager : RoleManager<Role>
    {
        public RoleManager(RoleStore store) : base (store)
        {

        }

        private QueryResult<Role> QueryRoles(int start, int count, string filter)
        {
            var results = this.Roles.ToList();
            
            if (!string.IsNullOrEmpty(filter))
            {
                //We could use dynamic linq here for more filtering options
                results = results.Where(u => u.Name.Contains(filter)).ToList();
            }
            int total = results.Count();
            results = results.Skip(start).Take(count).ToList();
            return new QueryResult<Role>(start, count, total, null, results);
        }

        public Task<QueryResult<Role>> QueryRolesAsync(int start, int count, string filter)
        {
            return Task.Run(() => QueryRoles(start, count, filter));
        }

        public QueryResult<TOut> ToDomain<TIn, TOut>(QueryResult<TIn> result, Func<TIn, TOut> resultConverter)
        {
            return new QueryResult<TOut>(result.Start, result.Count, result.Total, result.Filter, result.Items.Select(resultConverter));
        }

        /// <summary>
        /// Checks the role name is unique
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        internal bool CheckRoleNameInUse(string roleName)
        {            
            //If there is an existing role with this name then return false
            var existingRole = this.FindByName(roleName);
            return existingRole != null;

        }

    }
}
