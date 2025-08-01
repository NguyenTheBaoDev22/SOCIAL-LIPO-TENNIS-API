using Infrastructure.Extensions;
using Shared.Extensions;
using Shared.Interfaces;
using Shared.Results;
using System.Linq.Expressions;

namespace Infrastructure.Helpers
{
    public class PaginationHelper : IPaginationHelper
    {
        private const int DefaultPageSize = 10;
        private const int DefaultPageIndex = 1;

        public PaginatedResult<T> Paginate<T>(
            IQueryable<T> query,
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            string? search = null,
            string[]? searchFields = null,
            string? sortField = null,
            string? sortDirection = "asc")
        {
            pageIndex = pageIndex < 1 ? DefaultPageIndex : pageIndex;
            pageSize = pageSize <= 0 ? DefaultPageSize : pageSize;

            return query.ToPaginatedResult(
                pageIndex, pageSize, filter, search, searchFields, sortField, sortDirection);
        }

        public async Task<PaginatedResult<T>> PaginateAsync<T>(
            IQueryable<T> query,
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            string? search = null,
            string[]? searchFields = null,
            string? sortField = null,
            string? sortDirection = "asc")
        {
            pageIndex = pageIndex < 1 ? DefaultPageIndex : pageIndex;
            pageSize = pageSize <= 0 ? DefaultPageSize : pageSize;

            return await query.ToPaginatedResultAsync(
                pageIndex, pageSize, filter, search, searchFields, sortField, sortDirection);
        }
    }
}
