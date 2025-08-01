using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using AutoMapper;
using Core;
using Shared.Results;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Applications.Services.Implementations
{
    public class BaseService<TDto, TEntity> : IBaseService<TDto, TEntity>
     where TEntity : Audit
     where TDto : class
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public BaseService(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<BaseResponse<TDto>> GetByIdAsync(Guid id)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return BaseResponse<TDto>.Error("Entity not found", "404", traceId);

            return BaseResponse<TDto>.Success(_mapper.Map<TDto>(entity), "Retrieved successfully", "00");
        }

        public virtual async Task<BaseResponse<IEnumerable<TDto>>> GetAllAsync()
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

            var entities = await _repository.GetAllPaginatedAsync();
            return BaseResponse<IEnumerable<TDto>>.Success(_mapper.Map<IEnumerable<TDto>>(entities), "List retrieved", "00");
        }

        public virtual async Task<BaseResponse<TDto>> CreateAsync(TDto dto)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

            if (dto == null)
                return BaseResponse<TDto>.Error("Input is null", "400", traceId);

            try
            {
                var entity = _mapper.Map<TEntity>(dto);
                await _repository.AddAsync(entity);
                await _repository.SaveChangesAsync();

                var createdDto = _mapper.Map<TDto>(entity);
                return BaseResponse<TDto>.Success(createdDto, "Created successfully", "00");
            }
            catch (Exception ex)
            {
                return BaseResponse<TDto>.Error($"An error occurred: {ex.Message}", "500", traceId);
            }
        }
        // Phương thức tạo nhiều đối tượng cùng lúc
        public async Task<BaseResponse<IEnumerable<TDto>>> CreateManyAsync(IEnumerable<TDto> dtos)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

            if (dtos == null || !dtos.Any())
                return BaseResponse<IEnumerable<TDto>>.Error("Input is null or empty", "400", traceId);

            // Giới hạn số lượng items tối đa là 100
            if (dtos.Count() > 100)
                return BaseResponse<IEnumerable<TDto>>.Error("Cannot add more than 100 items", "400", traceId);

            try
            {
                // Chuyển đổi các DTO thành entity
                var entities = _mapper.Map<IEnumerable<TEntity>>(dtos);

                // Thêm vào repository
                foreach (var entity in entities)
                {
                    await _repository.AddAsync(entity);
                }

                // Lưu vào DB
                await _repository.SaveChangesAsync();

                // Chuyển đổi lại thành DTO và trả về
                var createdDtos = _mapper.Map<IEnumerable<TDto>>(entities);
                return BaseResponse<IEnumerable<TDto>>.Success(createdDtos, "Created successfully", "00");
            }
            catch (Exception ex)
            {
                return BaseResponse<IEnumerable<TDto>>.Error($"An error occurred: {ex.Message}", "500", traceId);
            }
        }
        public virtual async Task<BaseResponse<TDto>> UpdateAsync(Guid id, TDto dto)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

            if (dto == null)
                return BaseResponse<TDto>.Error("Input is null", "400", traceId);

            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return BaseResponse<TDto>.Error("Entity not found", "404", traceId);

            try
            {
                _mapper.Map(dto, entity);
                _repository.Update(entity);
                await _repository.SaveChangesAsync();

                return BaseResponse<TDto>.Success(_mapper.Map<TDto>(entity), "Updated successfully", "00");
            }
            catch (Exception ex)
            {
                return BaseResponse<TDto>.Error($"An error occurred: {ex.Message}", "500", traceId);
            }
        }

        public virtual async Task<BaseResponse<bool>> DeleteAsync(Guid id)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return BaseResponse<bool>.Error("Entity not found", "404", traceId);

            try
            {
                _repository.Delete(entity);
                await _repository.SaveChangesAsync();

                return BaseResponse<bool>.Success(true, "Deleted successfully", "00");
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.Error($"An error occurred: {ex.Message}", "500", traceId);
            }
        }

        public virtual async Task<BaseResponse<IEnumerable<TDto>>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

            var results = await _repository.FindAsync(predicate);
            return BaseResponse<IEnumerable<TDto>>.Success(_mapper.Map<IEnumerable<TDto>>(results), "Filtered list", "00");
        }

        public virtual async Task<bool> ExistsAsync(Guid id)
        {
            return await _repository.AnyAsync(x => x.Id == id);
        }
        // Phương thức phân trang cho GetAll
        public async Task<BaseResponse<PaginatedResult<TDto>>> GetAllPaginatedAsync(
       int pageIndex = 1,  // Mặc định pageIndex là 1
       int pageSize = 10,  // Mặc định pageSize là 10
       Expression<Func<TEntity, bool>>? filter = null,
       string? search = null,
       string[]? searchFields = null,
       string? sortField = null,
       string? sortDirection = "asc")
        {
            // Giới hạn pageSize tối đa là 100
            pageSize = pageSize > 100 ? 100 : pageSize;

            // Lấy dữ liệu từ repository, sử dụng AsQueryable() để lấy IQueryable
            IQueryable<TEntity> query = _repository.AsQueryable();

            // Áp dụng filter nếu có
            if (filter != null)
                query = query.Where(filter);

            // Sử dụng Dynamic LINQ để sắp xếp nếu có sortField
            if (!string.IsNullOrEmpty(sortField))
            {
                // Tạo biểu thức sắp xếp cho Dynamic LINQ
                var sortExpression = $"{sortField} {sortDirection}";  // Cấu trúc chuỗi sắp xếp

                // Dùng Dynamic LINQ để sắp xếp dữ liệu (OrderBy sẽ sử dụng chuỗi truyền vào)
                query = query.OrderBy(sortExpression);  // Dynamic LINQ OrderBy
            }

            // Tính toán tổng số bản ghi
            var totalCount = await _repository.CountAsync();  // Lấy tổng số bản ghi

            // Phân trang dữ liệu
            var items = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            // Chuyển đổi từ TEntity sang TDto
            var mappedItems = _mapper.Map<List<TDto>>(items);

            // Tạo PaginatedResult
            var paginatedResult = PaginatedResult<TDto>.Create(mappedItems, pageIndex, pageSize, totalCount);

            return BaseResponse<PaginatedResult<TDto>>.Success(paginatedResult, "List retrieved", "00");
        }




    }

}
