using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Shared.Results.Extensions
{
    public static class PaginationProjectToExtensions
    {
        public static async Task<PaginatedResult<TDestination>> ToPaginatedListProjectedAsync<TSource, TDestination>(
            this IQueryable<TSource> query,
            IConfigurationProvider configuration,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var skip = (page - 1) * pageSize;

            var projected = await query
                .Skip(skip)
                .Take(pageSize)
                .ProjectTo<TDestination>(configuration)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<TDestination>
            {
                Items = projected,
                Meta = new PaginationMeta
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    HasNext = page < totalPages,
                    HasPrevious = page > 1
                }
            };
        }
    }
}
