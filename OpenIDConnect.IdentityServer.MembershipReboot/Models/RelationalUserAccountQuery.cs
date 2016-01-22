using System;
using System.Linq;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Relational;
using OpenIDConnect.Core.Constants;

namespace OpenIDConnect.IdentityServer.MembershipReboot.Models
{
    public class RelationalUserAccountQuery<TAccount>
        where TAccount : UserAccount
    {
        public static readonly Func<IQueryable<TAccount>, string, IQueryable<TAccount>> DefaultFilter;
        public static readonly Func<IQueryable<TAccount>, IQueryable<TAccount>> DefaultSort;

        static RelationalUserAccountQuery()
        {
            var filter = typeof(RelationalUserAccountQuery<TAccount>).GetMethod("Filter");
            filter = filter.MakeGenericMethod(typeof(TAccount));
            DefaultFilter = (Func<IQueryable<TAccount>, string, IQueryable<TAccount>>)filter.CreateDelegate(typeof(Func<IQueryable<TAccount>, string, IQueryable<TAccount>>));

            var sort = typeof(RelationalUserAccountQuery<TAccount>).GetMethod("Sort");
            sort = sort.MakeGenericMethod(typeof(TAccount));
            DefaultSort = (Func<IQueryable<TAccount>, IQueryable<TAccount>>)sort.CreateDelegate(typeof(Func<IQueryable<TAccount>, IQueryable<TAccount>>));
        }

        public static string NameClaimType = ClaimTypes.Name;

        public static IQueryable<RAccount> Filter<RAccount>(IQueryable<RAccount> query, string filter)
            where RAccount : RelationalUserAccount
        {
            var results =
                from acct in query
                let claims = (from claim in acct.ClaimCollection
                              where claim.Type == NameClaimType && claim.Value.Contains(filter)
                              select claim)
                where
                    acct.Username.Contains(filter) || claims.Any()
                select acct;

            return results;
        }

        public static IQueryable<RAccount> Sort<RAccount>(IQueryable<RAccount> query)
            where RAccount : RelationalUserAccount
        {
            var results =
                from acct in query
                let display = (from claim in acct.ClaimCollection
                               where claim.Type == NameClaimType
                               select claim.Value).FirstOrDefault()
                orderby display ?? acct.Username
                select acct;

            return results;
        }
    }
}
