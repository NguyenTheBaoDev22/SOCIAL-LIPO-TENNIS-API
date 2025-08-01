using Applications.DTOs;
using Applications.Interfaces.Repositories;
using Applications.Services.Interfaces;
using AutoMapper;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Results;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Applications.Services.Implementations
{
    public class ProvinceService : BaseService<ProvinceDto, Province>, IProvinceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProvinceService(
            IProvinceRepository provinceRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(provinceRepository, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Tìm Province theo mã
        public async Task<BaseResponse<ProvinceDto>> GetByCodeAsync(string code)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

            // Lấy province theo mã
            var province = await _unitOfWork.ProvinceRepositories.GetByCodeAsync(code);
            if (province == null)
                return BaseResponse<ProvinceDto>.Error("Province not found", "404", traceId);

            // Chuyển từ entity sang DTO
            return BaseResponse<ProvinceDto>.Success(_mapper.Map<ProvinceDto>(province), "Retrieved successfully", "00");
        }
        // Lấy tất cả Province và chuyển sang ProvinceRes
        //public async Task<BaseResponse<IEnumerable<ProvinceRes>>> GetAllAsync()
        //{
        //    var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

        //    // Lấy dữ liệu từ repository
        //    var provinces = await _unitOfWork.ProvinceRepositories.GetAllAsync();

        //    // Ánh xạ từ Province sang ProvinceRes
        //    var provinceResList = _mapper.Map<IEnumerable<ProvinceRes>>(provinces);

        //    return BaseResponse<IEnumerable<ProvinceRes>>.Success(provinceResList, "List retrieved", "00");
        //}
        public async Task<BaseResponse<PaginatedResult<ProvinceRes>>> GetAllAsync(
    int pageIndex = 1,  // Mặc định pageIndex là 1
    int pageSize = 10,  // Mặc định pageSize là 10
    Expression<Func<Province, bool>>? filter = null,  // Bộ lọc tùy chọn
    string? search = null,  // Từ khóa tìm kiếm (nếu cần)
    string[]? searchFields = null,  // Các trường tìm kiếm (nếu cần)
    string? sortField = null,  // Trường sắp xếp
    string? sortDirection = "asc"  // Hướng sắp xếp (mặc định là "asc")
)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

            // Lấy tất cả Province từ repository
            IQueryable<Province> query = _unitOfWork.ProvinceRepositories.AsQueryable();

            // Áp dụng filter nếu có
            if (filter != null)
                query = query.Where(filter);

            // Áp dụng tìm kiếm nếu có
            if (!string.IsNullOrEmpty(search) && searchFields != null && searchFields.Any())
            {
                // Ví dụ tìm kiếm theo các trường được chỉ định trong searchFields
                foreach (var field in searchFields)
                {
                    var parameter = Expression.Parameter(typeof(Province), "x");
                    var property = Expression.Property(parameter, field);
                    var searchExpression = Expression.Call(property, "Contains", null, Expression.Constant(search));
                    var lambda = Expression.Lambda<Func<Province, bool>>(searchExpression, parameter);

                    query = query.Where(lambda);
                }
            }

            // Sử dụng Dynamic LINQ để sắp xếp nếu có sortField
            if (!string.IsNullOrEmpty(sortField))
            {
                var sortExpression = $"{sortField} {sortDirection}";
                query = query.OrderBy(sortExpression);  // Dùng Dynamic LINQ để sắp xếp theo tên trường
            }

            // Tính toán tổng số bản ghi
            var totalCount = await _unitOfWork.ProvinceRepositories.CountAsync();  // Lấy tổng số bản ghi

            // Phân trang dữ liệu
            var provinces = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            // Ánh xạ từ Province sang ProvinceRes
            var provinceResList = _mapper.Map<List<ProvinceRes>>(provinces);

            // Tạo đối tượng PaginatedResult
            var paginatedResult = PaginatedResult<ProvinceRes>.Create(provinceResList, pageIndex, pageSize, totalCount);

            // Trả về kết quả dưới dạng BaseResponse
            return BaseResponse<PaginatedResult<ProvinceRes>>.Success(paginatedResult, "List retrieved", "00");
        }
        // Lấy tất cả Province kèm theo Communes
        // Lấy tất cả Province kèm theo Communes
        public async Task<BaseResponse<PaginatedResult<GetAllWithCommunesRes>>> GetAllWithCommunesAsync(
            int pageIndex = 1,
            int pageSize = 10,
            Expression<Func<Province, bool>>? filter = null,
            string? search = null,
            string[]? searchFields = null,
            string? sortField = null,
            string? sortDirection = "asc")
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

            // Lấy tất cả Province từ repository
            IQueryable<Province> query = _unitOfWork.ProvinceRepositories.AsQueryable();

            // Áp dụng filter nếu có
            if (filter != null)
                query = query.Where(filter);

            // Áp dụng tìm kiếm nếu có
            if (!string.IsNullOrEmpty(search) && searchFields != null && searchFields.Any())
            {
                // Ví dụ tìm kiếm theo các trường được chỉ định trong searchFields
                foreach (var field in searchFields)
                {
                    var parameter = Expression.Parameter(typeof(Province), "x");
                    var property = Expression.Property(parameter, field);
                    var searchExpression = Expression.Call(property, "Contains", null, Expression.Constant(search));
                    var lambda = Expression.Lambda<Func<Province, bool>>(searchExpression, parameter);

                    query = query.Where(lambda);
                }
            }

            // Sử dụng Dynamic LINQ để sắp xếp nếu có sortField
            if (!string.IsNullOrEmpty(sortField))
            {
                var sortExpression = $"{sortField} {sortDirection}";
                query = query.OrderBy(sortExpression);  // Dùng Dynamic LINQ để sắp xếp theo tên trường
            }

            // Tính toán tổng số bản ghi
            var totalCount = await _unitOfWork.ProvinceRepositories.CountAsync();

            // Phân trang dữ liệu
            var provinces = await query
                .Include(p => p.Communes) // Eager load the related Communes
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();  // Use ToListAsync to ensure the query is executed asynchronously

            // Ánh xạ từ Province sang GetAllWithCommunesRes
            var provinceResList = provinces.Select(p => new GetAllWithCommunesRes
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                DivisionType = p.DivisionType,
                Communes = p.Communes.Select(c => new CommuneInProvinceRes
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    DivisionType = c.DivisionType
                }).ToList()
            }).ToList();

            // Tạo đối tượng PaginatedResult
            var paginatedResult = PaginatedResult<GetAllWithCommunesRes>.Create(provinceResList, pageIndex, pageSize, totalCount);

            // Trả về kết quả dưới dạng BaseResponse
            return BaseResponse<PaginatedResult<GetAllWithCommunesRes>>.Success(paginatedResult, "List retrieved", "00");
        }

        // Các phương thức khác sẽ tự động được kế thừa từ BaseService
    }
}
