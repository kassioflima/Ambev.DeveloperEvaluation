using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Common;

/// <summary>
/// Helper class for query operations like filtering and sorting
/// </summary>
public static class QueryHelper
{
    /// <summary>
    /// Applies dynamic ordering to a queryable based on order string
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="query">The queryable to order</param>
    /// <param name="orderString">Order string in format "field1 asc, field2 desc"</param>
    /// <returns>Ordered queryable</returns>
    public static IQueryable<T> ApplyOrdering<T>(IQueryable<T> query, string? orderString)
    {
        if (string.IsNullOrWhiteSpace(orderString))
            return query;

        var orderParts = orderString.Split(',', StringSplitOptions.RemoveEmptyEntries);
        IOrderedQueryable<T>? orderedQuery = null;
        bool isFirst = true;

        foreach (var part in orderParts)
        {
            var trimmed = part.Trim();
            var spaceIndex = trimmed.LastIndexOf(' ');
            
            string fieldName;
            bool ascending = true;

            if (spaceIndex > 0)
            {
                fieldName = trimmed.Substring(0, spaceIndex).Trim();
                var direction = trimmed.Substring(spaceIndex + 1).Trim().ToLower();
                ascending = direction != "desc";
            }
            else
            {
                fieldName = trimmed;
            }

            var property = typeof(T).GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
                continue;

            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Property(parameter, property);
            var lambda = Expression.Lambda(propertyAccess, parameter);

            if (isFirst)
            {
                var methodName = ascending ? "OrderBy" : "OrderByDescending";
                var method = typeof(Queryable).GetMethods()
                    .First(m => m.Name == methodName && m.GetParameters().Length == 2);
                var genericMethod = method.MakeGenericMethod(typeof(T), property.PropertyType);
                orderedQuery = (IOrderedQueryable<T>)genericMethod.Invoke(null, new object[] { query, lambda })!;
                isFirst = false;
            }
            else
            {
                var methodName = ascending ? "ThenBy" : "ThenByDescending";
                var method = typeof(Queryable).GetMethods()
                    .First(m => m.Name == methodName && m.GetParameters().Length == 2);
                var genericMethod = method.MakeGenericMethod(typeof(T), property.PropertyType);
                orderedQuery = (IOrderedQueryable<T>)genericMethod.Invoke(null, new object[] { orderedQuery!, lambda })!;
            }
        }

        return orderedQuery ?? query;
    }

    /// <summary>
    /// Applies string filter with wildcard support
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="query">The queryable to filter</param>
    /// <param name="propertyName">Property name to filter</param>
    /// <param name="value">Filter value (supports * wildcard)</param>
    /// <returns>Filtered queryable</returns>
    public static IQueryable<T> ApplyStringFilter<T>(IQueryable<T> query, string propertyName, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return query;

        var property = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (property == null || property.PropertyType != typeof(string))
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Property(parameter, property);

        Expression filterExpression;

        if (value.StartsWith("*") && value.EndsWith("*"))
        {
            // Contains
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var constant = Expression.Constant(value.Trim('*'), typeof(string));
            filterExpression = Expression.Call(propertyAccess, containsMethod!, constant);
        }
        else if (value.StartsWith("*"))
        {
            // EndsWith
            var endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
            var constant = Expression.Constant(value.TrimStart('*'), typeof(string));
            filterExpression = Expression.Call(propertyAccess, endsWithMethod!, constant);
        }
        else if (value.EndsWith("*"))
        {
            // StartsWith
            var startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            var constant = Expression.Constant(value.TrimEnd('*'), typeof(string));
            filterExpression = Expression.Call(propertyAccess, startsWithMethod!, constant);
        }
        else
        {
            // Exact match
            var equalsMethod = typeof(string).GetMethod("Equals", new[] { typeof(string) });
            var constant = Expression.Constant(value, typeof(string));
            filterExpression = Expression.Call(propertyAccess, equalsMethod!, constant);
        }

        var lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
        return query.Where(lambda);
    }
}

