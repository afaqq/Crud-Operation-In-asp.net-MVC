using System;
using System.Linq;
using System.Linq.Expressions;

public static class IQueryableExtensions
{
    public static IOrderedQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var lambda = Expression.Lambda(property, parameter);
        var method = typeof(Queryable).GetMethods().Single(
            method => method.Name == "OrderBy" && method.IsGenericMethodDefinition
                && method.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.Type);

        return (IOrderedQueryable<T>)method.Invoke(null, new object[] { query, lambda });
    }

    public static IOrderedQueryable<T> OrderByDescendingDynamic<T>(this IQueryable<T> query, string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var lambda = Expression.Lambda(property, parameter);
        var method = typeof(Queryable).GetMethods().Single(
            method => method.Name == "OrderByDescending" && method.IsGenericMethodDefinition
                && method.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.Type);

        return (IOrderedQueryable<T>)method.Invoke(null, new object[] { query, lambda });
    }
}