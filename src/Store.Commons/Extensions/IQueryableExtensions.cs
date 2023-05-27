using System.Linq.Expressions;

namespace Store.Commons.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        public static IQueryable<T> Take<T>(this IQueryable<T> source, bool flag, int count)
        {
            if (flag)
            {
                return source.Take(count);
            }

            return source;
        }
    }
}
