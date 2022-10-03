using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using Newtonsoft.Json;
/// <summary>
/// Some useful extension methods for <see cref="IQueryable{T}"/>.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
    /// </summary>
    public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int skipCount, int maxResultCount)
    {
        if (query == null)
        {
            throw new ArgumentNullException("query");
        }

        return query.Skip(skipCount).Take(maxResultCount);
    }

    // /// <summary>
    // /// Used for paging with an <see cref="IPagedResultRequest"/> object.
    // /// </summary>
    // /// <param name="query">Queryable to apply paging</param>
    // /// <param name="pagedResultRequest">An object implements <see cref="IPagedResultRequest"/> interface</param>
    // public static IQueryable<T> PageBy<T>(this IQueryable<T> query, IPagedResultRequest pagedResultRequest)
    // {
    //     return query.PageBy(pagedResultRequest.SkipCount, pagedResultRequest.MaxResultCount);
    // }

    /// <summary>
    /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
    /// </summary>
    /// <param name="query">Queryable to apply filtering</param>
    /// <param name="condition">A boolean value</param>
    /// <param name="predicate">Predicate to filter the query</param>
    /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
    {
        return condition
            ? query.Where(predicate)
            : query;
    }

    /// <summary>
    /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
    /// </summary>
    /// <param name="query">Queryable to apply filtering</param>
    /// <param name="condition">A boolean value</param>
    /// <param name="predicate">Predicate to filter the query</param>
    /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
    {
        return condition
            ? query.Where(predicate)
            : query;
    }

    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> query, bool condition, Func<T, bool> predicate)
    {
        return condition ? query.Where(predicate) : query;
    }

}