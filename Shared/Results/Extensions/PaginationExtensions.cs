using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Results.Extensions
{
    public static class PaginationExtensions
    {
        public static async Task<PaginatedResult<TDestination>> ToPaginatedListAsync<TSource, TDestination>(
            this IQueryable<TSource> query,
            IMapper mapper,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var skip = (page - 1) * pageSize;

            var items = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var mappedItems = mapper.Map<List<TDestination>>(items);

            return new PaginatedResult<TDestination>
            {
                Items = mappedItems,
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
