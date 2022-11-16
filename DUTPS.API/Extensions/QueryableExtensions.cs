using System.Linq.Expressions;
using DUTPS.Commons;

namespace DUTPS.API.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, ParamsSearch sortModels)
        {
            try
            {
                var expression = source.Expression;
                var parameter = Expression.Parameter(typeof(T), "x");
                Expression selector = parameter;
                foreach (var member in sortModels.OrderBy.Split('.'))
                {
                    selector = Expression.PropertyOrField(selector, member);
                }
                var method = string.Equals(sortModels.Order, "DESC", StringComparison.OrdinalIgnoreCase) ? "OrderByDescending" : "OrderBy";
                expression = Expression.Call(typeof(Queryable), method,
                    new Type[] { source.ElementType, selector.Type },
                    expression, Expression.Quote(Expression.Lambda(selector, parameter)));
                return source.Provider.CreateQuery<T>(expression);
            }
            catch (Exception)
            {
                return source;
            }

        }
    }
}