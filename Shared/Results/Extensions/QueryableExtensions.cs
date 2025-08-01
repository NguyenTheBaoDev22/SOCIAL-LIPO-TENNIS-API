//using Microsoft.EntityFrameworkCore;
//using Shared.Results;
//using System.Linq.Dynamic.Core; // ✅ Bắt buộc
//using System.Linq.Expressions;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

//namespace Shared.Extensions
//{
//    public static class QueryableExtensions
//    {
//        public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(
//            this IQueryable<T> query,
//            int pageIndex,
//            int pageSize,
//            Expression<Func<T, bool>>? filter = null,
//            string? search = null,
//            string[]? searchFields = null,
//            string? sortField = null,
//            string? sortDirection = "asc")
//        {
//            // Bước 1: Filter
//            if (filter != null)
//                query = query.Where(filter);

//            // Bước 2: Search keyword
//            if (!string.IsNullOrWhiteSpace(search) && searchFields != null && searchFields.Length > 0)
//            {
//                // Tạo biểu thức theo Dynamic LINQ
//                var predicate = string.Join(" OR ", searchFields.Select(f => $"{f}.Contains(@0)"));
//                query = query.Where(predicate, search);
//            }

//            // Bước 3: Sort
//            if (!string.IsNullOrWhiteSpace(sortField))
//            {
//                var ordering = $"{sortField} {(sortDirection?.ToLower() == "desc" ? "descending" : "ascending")}";
//                query = query.OrderBy(ordering);
//            }

//            // Bước 4: Count + page
//            var totalCount = await query.CountAsync();
//            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

//            return PaginatedResult<T>.Create(items, pageIndex, pageSize, totalCount);
//        }
//    }
//}
using Shared.Results;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Shared.Extensions
{
    public static class QueryableExtensions
    {
        public static PaginatedResult<T> ToPaginatedResult<T>(
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

            var totalCount = query.Count();
            var items = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return PaginatedResult<T>.Create(items, pageIndex, pageSize, totalCount);
        }
    }
}


///Ví dụ cách dùng
//    var result = await dbContext.Products
//.AsQueryable()
//.ToPaginatedResultAsync(
//    pageIndex: 1,
//    pageSize: 20,
//    filter: p => p.CategoryId == selectedCategory,
//    search: "apple",
//    searchFields: new[] { "Name", "Description" },
//    sortField: "CreatedAt",
//    sortDirection: "desc"
//);

//var totalCount = await query.CountAsync();
//var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

