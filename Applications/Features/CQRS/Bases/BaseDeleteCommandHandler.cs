//using Applications.Interfaces.Services;
//using AutoMapper;
//using MediatR;
//using Shared.Results;
//using Serilog;
//using System.Threading;
//using System.Threading.Tasks;
//using Applications.CQRS.Bases;
//using Core;

//namespace Applications.Features.CQRS.Bases
//{
//    // BaseDeleteCommandHandler kế thừa BaseCommandHandler với tham số là BaseDeleteCommand
//    public class BaseDeleteCommandHandler<TDto, TEntity>
//        : BaseCommandHandler<BaseDeleteCommand, TDto>  // BaseDeleteCommand là TCommand
//        where TEntity : Audit
//        where TDto : class
//    {
//        private readonly IBaseService<TDto, TEntity> _service;

//        public BaseDeleteCommandHandler(
//            IBaseService<TDto, TEntity> service,
//            IMapper mapper)
//            : base(mapper)
//        {
//            _service = service;
//        }

//        // Override để xử lý logic xóa
//        protected override async Task<BaseResponse<TDto>> ExecuteAsync(
//            BaseDeleteCommand request,
//            string traceId,
//            CancellationToken cancellationToken)
//        {
//            Log.Information("🗑️ [{TraceId}] Deleting entity with Id: {Id}", traceId, request.Id);

//            var result = await _service.DeleteAsync(request.Id);

//            if (result.IsSuccess)
//            {
//                Log.Information("✅ [{TraceId}] Deleted entity successfully", traceId);
//            }
//            else
//            {
//                Log.Warning("⚠️ [{TraceId}] Failed to delete entity. Message: {Message}, Code: {Code}", traceId, result.Message, result.Code);
//            }

//            return result;
//        }
//    }
//}
