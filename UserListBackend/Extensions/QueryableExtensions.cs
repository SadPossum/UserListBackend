using System.Linq.Expressions;
using System.Reflection;
using UserListBackend.Models.LogicModels;
using Microsoft.EntityFrameworkCore.Query;

namespace UserListBackend.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, List<SortCriteria> sortCriterias)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (sortCriterias == null || sortCriterias.Count == 0)
            {
                return source;
            }

            IQueryable<T> orderedSource = source;

            ParameterExpression param = Expression.Parameter(typeof(T), "x");

            foreach (SortCriteria sortCriteria in sortCriterias)
            {
                if (sortCriteria == null)
                {
                    throw new NullReferenceException(nameof(sortCriteria));
                }

                if (sortCriteria.PropertyName == null)
                {
                    throw new NullReferenceException(nameof(sortCriteria.PropertyName));
                }

                PropertyInfo? property = typeof(T).GetProperty(sortCriteria.PropertyName);

                if (property == null)
                {
                    throw new ArgumentException($"Property '{sortCriteria.PropertyName}' not found on type '{typeof(T).FullName}'");
                }

                Expression propertyAccessExpression = Expression.MakeMemberAccess(param, property);

                LambdaExpression orderByExpression = Expression.Lambda(propertyAccessExpression, param);

                MethodInfo orderByMethod = sortCriteria.IsDescending
                    ? QueryableMethods.OrderByDescending
                    : QueryableMethods.OrderBy;

                MethodCallExpression resultExp = Expression.Call(null, orderByMethod.MakeGenericMethod(typeof(T), property.PropertyType),
                    orderedSource.Expression, Expression.Quote(orderByExpression));

                orderedSource = orderedSource.Provider.CreateQuery<T>(resultExp);
            }

            return orderedSource;
        }
    }
}
