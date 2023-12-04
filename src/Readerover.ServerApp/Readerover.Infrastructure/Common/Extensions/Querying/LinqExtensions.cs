using Readerover.Application.Common.Models.Querying;

namespace Readerover.Infrastructure.Common.Extensions.Querying;

public static class LinqExtensions
{
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> source, FilterPagination pagination)
        => source.Skip((pagination.PageToken - 1) * pagination.PageSize).Take(pagination.PageSize);

    public static IEnumerable<T> ApplyPagination<T>(this IEnumerable<T> source, FilterPagination pagination)
           => source.Skip((pagination.PageToken - 1) * pagination.PageSize).Take(pagination.PageSize);
}
