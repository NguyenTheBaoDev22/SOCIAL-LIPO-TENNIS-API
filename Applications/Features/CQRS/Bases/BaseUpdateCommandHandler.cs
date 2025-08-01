using Applications.CQRS.Bases;
using Applications.Interfaces.Services;
using AutoMapper;
using Core;
using Serilog;
using Shared.Results;

namespace Applications.Features.CQRS.Bases
{
    /// <summary>
    /// Base handler cho UpdateCommand CQRS.
    /// </summary>
    /// <typeparam name="TDto">Kiểu DTO</typeparam>
    /// <typeparam name="TEntity">Kiểu Entity kế thừa Audit</typeparam>
    public class BaseUpdateCommandHandler<TDto, TEntity>
        : BaseCommandHandler<BaseUpdateCommand<TDto>, TDto>
        where TEntity : Audit
        where TDto : class
    {
        private readonly IBaseService<TDto, TEntity> _service;

        public BaseUpdateCommandHandler(
            IBaseService<TDto, TEntity> service,
            IMapper mapper)
            : base(mapper)
        {
            _service = service;
        }

        /// <summary>
        /// Thực hiện cập nhật entity từ DTO và Id.
        /// </summary>
        protected override async Task<BaseResponse<TDto>> ExecuteAsync(
            BaseUpdateCommand<TDto> request,
            string traceId,
            CancellationToken cancellationToken)
        {
            Log.Information("✏️ [{TraceId}] Updating entity with Id: {Id}, Data: {@Dto}", traceId, request.Id, request.Data);

            var result = await _service.UpdateAsync(request.Id, request.Data);

            if (result.IsSuccess)
            {
                Log.Information("✅ [{TraceId}] Updated entity successfully {@Result}", traceId, result.Data);
            }
            else
            {
                Log.Warning("⚠️ [{TraceId}] Failed to update entity. Message: {Message}, Code: {Code}", traceId, result.Message, result.Code);
            }

            return result;
        }
    }
}
