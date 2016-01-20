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

        private QueryResult<Role> QueryRoles(int start, int count,string filter)
        {
            var results = this.Roles.ToList();
            List<Role> selectedRoles = new List<Role>();
            if (!string.IsNullOrEmpty(filter))
            {
                //We could use dynamic linq here for more filtering options
                selectedRoles = selectedRoles.Where(u => u.Name.Contains(filter)).ToList();
            }
            int total = results.Count();
            selectedRoles = results.Skip(start).Take(count).ToList();
            return new QueryResult<Role>(start, count, total, null, selectedRoles);
        }

        public Task<QueryResult<Role>> QueryRolesAsync(int start, int count, string filter)
        {
            return Task.Run(() => QueryRoles(start, count, filter));
        }

        public QueryResult<TOut> ToDomain<TIn, TOut>(QueryResult<TIn> result, Func<TIn, TOut> resultConverter)
        {
            return new QueryResult<TOut>(result.Start, result.Count, result.Total, result.Filter, result.Items.Select(resultConverter));
        }
    }
}
