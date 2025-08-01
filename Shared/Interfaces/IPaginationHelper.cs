using Shared.Results;
using System.Linq.Expressions;

namespace Shared.Interfaces
{
    public interface IPaginationHelper
    {
        PaginatedResult<T> Paginate<T>(
            IQueryable<T> query,
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            string? search = null,
            string[]? searchFields = null,
            string? sortField = null,
            string? sortDirection = "asc");

        Task<PaginatedResult<T>> PaginateAsync<T>(
            IQueryable<T> query,
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            string? search = null,
            string[]? searchFields = null,
            string? sortField = null,
            string? sortDirection = "asc");
    }
}
