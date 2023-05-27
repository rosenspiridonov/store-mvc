using System.Linq.Expressions;

namespace Store.Commons.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }
    }
}
