using Applications.CQRS.Bases;
using Applications.Interfaces.Services;
using AutoMapper;
using Core;
using Serilog;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.CQRS.Bases
{
    /// <summary>
    /// Base handler cho CreateManyCommand CQRS (bulk insert).
    /// </summary>
    /// <typeparam name="TDto">Kiểu DTO</typeparam>
    /// <typeparam name="TEntity">Entity kế thừa Audit</typeparam>
    public class BaseCreateManyCommandHandler<TDto, TEntity>
        : BaseCommandHandler<BaseCreateManyCommand<TDto>, IEnumerable<TDto>>
        where TEntity : Audit
        where TDto : class
    {
        private readonly IBaseService<TDto, TEntity> _service;

        public BaseCreateManyCommandHandler(
            IBaseService<TDto, TEntity> service,
            IMapper mapper)
            : base(mapper)
        {
            _service = service;
        }

        protected override async Task<BaseResponse<IEnumerable<TDto>>> ExecuteAsync(
            BaseCreateManyCommand<TDto> request,
            string traceId,
            CancellationToken cancellationToken)
        {
            Log.Information("➕ [{TraceId}] Creating multiple entities ({Count} items)", traceId, request.Data.Count());

            var result = await _service.CreateManyAsync(request.Data);

            if (result.IsSuccess)
            {
                Log.Information("✅ [{TraceId}] Created {Count} entities successfully", traceId, result.Data.Count());
            }
            else
            {
                Log.Warning("⚠️ [{TraceId}] Failed to create entities. Message: {Message}, Code: {Code}", traceId, result.Message, result.Code);
            }

            return result;
        }
    }
}
