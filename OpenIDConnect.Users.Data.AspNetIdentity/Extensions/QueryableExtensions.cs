namespace OpenIDConnect.Users.Data.AspNetIdentity.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    internal static class QueryableExtensions
    {
        public static IQueryable<TSource> WhereIf<TSource>(
            this IQueryable<TSource> source,
            Func<bool> predicate,
            Expression<Func<TSource, bool>> filterPredicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return !predicate() ? source : source.Where(filterPredicate);
        }
    }
}