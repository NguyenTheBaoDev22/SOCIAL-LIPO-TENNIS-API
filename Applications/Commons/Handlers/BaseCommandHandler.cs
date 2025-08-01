using Applications.Interfaces.Repositories;
using AutoMapper;
using Core;
using MediatR;

namespace Applications.Commons.Handlers
{
    public abstract class BaseCommandHandler<TCommand, TEntity, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : IRequest<TResponse>
        where TEntity : Audit
    {
        protected readonly IBaseRepository<TEntity> Repository;
        protected readonly IMapper Mapper;

        protected BaseCommandHandler(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public abstract Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);

        /// <summary>
        /// Thêm mới entity từ DTO
        /// </summary>
        protected virtual async Task<TResponse> CreateAndSaveAsync<TDto>(TDto dto)
            where TDto : class
        {
            var entity = Mapper.Map<TEntity>(dto);
            await Repository.AddAsync(entity);
            var success = await Repository.SaveChangesAsync();
            return BuildResponse(success, entity);
        }

        /// <summary>
        /// Cập nhật entity đã có với DTO mới
        /// </summary>
        protected virtual async Task<TResponse> UpdateAndSaveAsync<TDto>(Guid id, TDto dto)
            where TDto : class
        {
            var entity = await Repository.GetByIdAsync(id);
            if (entity == null)
                return BuildResponse(false, null, "Entity not found");

            Mapper.Map(dto, entity);
            Repository.Update(entity);
            var success = await Repository.SaveChangesAsync();
            return BuildResponse(success, entity);
        }

        /// <summary>
        /// Xoá mềm entity
        /// </summary>
        protected virtual async Task<TResponse> DeleteAndSaveAsync(Guid id)
        {
            var entity = await Repository.GetByIdAsync(id);
            if (entity == null)
                return BuildResponse(false, null, "Entity not found");

            Repository.Delete(entity);
            var success = await Repository.SaveChangesAsync();
            return BuildResponse(success, entity);
        }

        /// <summary>
        /// Tạo response tuỳ theo hệ thống (override để tuỳ biến theo TResponse cụ thể)
        /// </summary>
        protected virtual TResponse BuildResponse(bool success, TEntity? entity = null, string? errorMessage = null)
        {
            // Nếu TResponse là bool
            if (typeof(TResponse) == typeof(bool))
                return (TResponse)(object)success;

            // Nếu TResponse là kiểu BaseResponse
            if (typeof(TResponse).Name.StartsWith("BaseResponse"))
            {
                var responseType = typeof(TResponse).GetGenericArguments().FirstOrDefault();
                var mappedData = entity != null && responseType != null
                    ? Mapper.Map(entity, entity.GetType(), responseType)
                    : null;

                var method = typeof(Shared.Results.BaseResponse<>)
                    .MakeGenericType(responseType ?? typeof(object))
                    .GetMethod(success ? "Success" : "Error", new[] { responseType ?? typeof(object) });

                return (TResponse)(method?.Invoke(null, success
                    ? new[] { mappedData! }
                    : new object[] { errorMessage ?? "Unknown error" })!);
            }

            // Mặc định
            throw new InvalidOperationException("Unsupported response type in BaseCommandHandler");
        }
    }
}
