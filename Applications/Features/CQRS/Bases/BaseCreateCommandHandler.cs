using Applications.CQRS.Bases;
using Applications.Interfaces.Services;
using AutoMapper;
using Core;
using Serilog;
using Shared.Results;

namespace Applications.Features.CQRS.Bases
{
    /// <summary>
    /// Base handler cho CreateCommand CQRS.
    /// Thực hiện tạo entity từ DTO, log quá trình xử lý qua Serilog.
    /// </summary>
    /// <typeparam name="TDto">Kiểu DTO</typeparam>
    /// <typeparam name="TEntity">Kiểu Entity kế thừa Audit</typeparam>
    public class BaseCreateCommandHandler<TDto, TEntity>
        : BaseCommandHandler<BaseCreateCommand<TDto>, TDto>
        where TEntity : Audit
        where TDto : class
    {
        private readonly IBaseService<TDto, TEntity> _service;

        public BaseCreateCommandHandler(
            IBaseService<TDto, TEntity> service,
            IMapper mapper)
            : base(mapper)
        {
            _service = service;
        }

        /// <summary>
        /// Thực hiện logic tạo mới entity từ DTO và log kết quả.
        /// </summary>
        protected override async Task<BaseResponse<TDto>> ExecuteAsync(
            BaseCreateCommand<TDto> request,
            string traceId,
            CancellationToken cancellationToken)
        {
            Log.Information("➡️ [{TraceId}] Creating new entity {@Dto}", traceId, request.Data);

            var result = await _service.CreateAsync(request.Data);

            if (result.IsSuccess)
            {
                Log.Information("✅ [{TraceId}] Created entity successfully {@Result}", traceId, result.Data);
            }
            else
            {
                Log.Warning("⚠️ [{TraceId}] Failed to create entity. Message: {Message}, Code: {Code}",
                    traceId, result.Message, result.Code);
            }

            return result;
        }
    }
}
