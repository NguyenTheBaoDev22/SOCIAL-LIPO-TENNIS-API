using AutoMapper;
using MediatR;
using Serilog;
using Shared.Results;
using System.Diagnostics;

namespace Applications.CQRS.Bases
{
    public abstract class BaseCommandHandler<TCommand, TResult>
        : IRequestHandler<TCommand, BaseResponse<TResult>>
        where TCommand : IRequest<BaseResponse<TResult>>
    {
        protected readonly IMapper _mapper;

        protected BaseCommandHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<BaseResponse<TResult>> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();
            Log.Information("➡️ [{TraceId}] Handling command: {Command}", traceId, typeof(TCommand).Name);

            try
            {
                var result = await ExecuteAsync(request, traceId, cancellationToken);
                Log.Information("✅ [{TraceId}] Success command: {Command}", traceId, typeof(TCommand).Name);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "❌ [{TraceId}] Unhandled error in {Command}", traceId, typeof(TCommand).Name);
                return BaseResponse<TResult>.Error("Internal error", "500", traceId);
            }
        }

        protected abstract Task<BaseResponse<TResult>> ExecuteAsync(TCommand request, string traceId, CancellationToken cancellationToken);
    }
}
