using Core;
using Shared.Results;
using System.Linq.Expressions;

namespace Applications.Interfaces.Services
{
    public interface IBaseService<TDto, TEntity>
        where TEntity : Audit
        where TDto : class
    {
        Task<BaseResponse<TDto>> GetByIdAsync(Guid id);
        Task<BaseResponse<IEnumerable<TDto>>> GetAllAsync();
        Task<BaseResponse<TDto>> CreateAsync(TDto dto);
        Task<BaseResponse<TDto>> UpdateAsync(Guid id, TDto dto);
        Task<BaseResponse<bool>> DeleteAsync(Guid id);
        Task<BaseResponse<IEnumerable<TDto>>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> ExistsAsync(Guid id);
        Task<BaseResponse<PaginatedResult<TDto>>> GetAllPaginatedAsync(
       int pageIndex = 1,  // Mặc định pageIndex là 1
       int pageSize = 10,  // Mặc định pageSize là 10
       Expression<Func<TEntity, bool>>? filter = null,
       string? search = null,
       string[]? searchFields = null,
       string? sortField = null,
       string? sortDirection = "asc");
        Task<BaseResponse<IEnumerable<TDto>>> CreateManyAsync(IEnumerable<TDto> dtos);
    }
}
