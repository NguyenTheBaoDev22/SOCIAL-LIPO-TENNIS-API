using Microsoft.EntityFrameworkCore;
using Shared.Results;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Infrastructure.Extensions
{
    public static class QueryableEfExtensions
    {
        public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(
            this IQueryable<T> query,
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            string? search = null,
            string[]? searchFields = null,
            string? sortField = null,
            string? sortDirection = "asc")
        {
            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(search) && searchFields?.Length > 0)
            {
                var predicate = string.Join(" OR ", searchFields.Select(f => $"{f}.Contains(@0)"));
                query = query.Where(predicate, search);
            }

            if (!string.IsNullOrWhiteSpace(sortField))
            {
                var ordering = $"{sortField} {(sortDirection?.ToLower() == "desc" ? "descending" : "ascending")}";
                query = query.OrderBy(ordering);
            }

            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return PaginatedResult<T>.Create(items, pageIndex, pageSize, totalCount);
        }
    }
}
//Cách dùng
//var result = await _dbContext.Users
//    .Where(u => u.IsActive)
//    .ToPaginatedResultAsync(
//        pageIndex: 1,
//        pageSize: 10,
//        search: "admin",
//        searchFields: new[] { "FullName", "Email" },
//        sortField: "CreatedAt",
//        sortDirection: "desc"
//    );

//Dùng với danh sách List<T>:
//    var inMemoryUsers = new List<User>
//{
//    new User { FullName = "Nguyen Van A", Email = "a@example.com" },
//    new User { FullName = "Tran Thi B", Email = "b@example.com" },
//    // ...
//}.AsQueryable();

//var result = inMemoryUsers.ToPaginatedResult(
//    pageIndex: 1,
//    pageSize: 5,
//    search: "Nguyen",
//    searchFields: new[] { "FullName" },
//    sortField: "FullName",
//    sortDirection: "asc"
//);
