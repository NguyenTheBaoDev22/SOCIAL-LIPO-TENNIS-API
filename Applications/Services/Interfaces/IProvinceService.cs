using Applications.DTOs;
using Applications.Interfaces.Services;
using Core.Entities;
using Shared.Results;
using System.Linq.Expressions;

namespace Applications.Services.Interfaces
{
    public interface IProvinceService : IBaseService<ProvinceDto, Province>
    {
        // Thêm phương thức đặc thù nếu cần
        Task<BaseResponse<ProvinceDto>> GetByCodeAsync(string code);
        //Task<BaseResponse<IEnumerable<ProvinceRes>>> GetAllAsync();  // Thêm phương thức trả về ProvinceRes
        Task<BaseResponse<PaginatedResult<ProvinceRes>>> GetAllAsync(
    int pageIndex = 1,  // Mặc định pageIndex là 1
    int pageSize = 10,  // Mặc định pageSize là 10
    Expression<Func<Province, bool>>? filter = null,  // Bộ lọc tùy chọn
    string? search = null,  // Từ khóa tìm kiếm (nếu cần)
    string[]? searchFields = null,  // Các trường tìm kiếm (nếu cần)
    string? sortField = null,  // Trường sắp xếp
    string? sortDirection = "asc"  // Hướng sắp xếp (mặc định là "asc")
);
        Task<BaseResponse<PaginatedResult<GetAllWithCommunesRes>>> GetAllWithCommunesAsync(
            int pageIndex = 1,
            int pageSize = 10,
            Expression<Func<Province, bool>>? filter = null,
            string? search = null,
            string[]? searchFields = null,
            string? sortField = null,
            string? sortDirection = "asc");
    }
}
