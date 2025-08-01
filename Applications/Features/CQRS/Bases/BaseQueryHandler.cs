using AutoMapper;
using MediatR;
using Serilog;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.CQRS.Bases
{
    public abstract class BaseQueryHandler<TQuery, TResult>
        : IRequestHandler<TQuery, BaseResponse<TResult>>
        where TQuery : IRequest<BaseResponse<TResult>>
    {
        protected readonly IMapper _mapper;

        protected BaseQueryHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<BaseResponse<TResult>> Handle(TQuery request, CancellationToken cancellationToken)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();
            Log.Information("🔍 [{TraceId}] Handling query: {Query}", traceId, typeof(TQuery).Name);

            try
            {
                var result = await ExecuteAsync(request, traceId, cancellationToken);
                Log.Information("✅ [{TraceId}] Query completed: {Query}", traceId, typeof(TQuery).Name);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "❌ [{TraceId}] Error handling query {Query}", traceId, typeof(TQuery).Name);
                return BaseResponse<TResult>.Error("Internal error", "500", traceId);
            }
        }

        protected abstract Task<BaseResponse<TResult>> ExecuteAsync(TQuery request, string traceId, CancellationToken cancellationToken);
    }
}
