//using Applications.Interfaces.Repositories;
//using AutoMapper;
//using Core;
//using MediatR;
//using Serilog;
//using Shared.Results;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Text;
//using System.Threading.Tasks;

//namespace Applications.Features.CQRS.Bases
//{
//    public abstract class BasePaginatedQueryHandler<TQuery, TDto, TEntity>
//        : IRequestHandler<TQuery, BaseResponse<PaginatedResult<TDto>>>
//        where TQuery : BasePaginatedQuery<TDto>
//        where TEntity : Audit
//        where TDto : class
//    {
//        protected readonly IBaseRepository<TEntity> _repository;
//        protected readonly IMapper _mapper;

//        protected BasePaginatedQueryHandler(IBaseRepository<TEntity> repository, IMapper mapper)
//        {
//            _repository = repository;
//            _mapper = mapper;
//        }

//        public async Task<BaseResponse<PaginatedResult<TDto>>> Handle(TQuery request, CancellationToken cancellationToken)
//        {
//            var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString();
//            Log.Information("📄 [{TraceId}] Executing paginated query: {Query}", traceId, typeof(TQuery).Name);

//            try
//            {
//                var query = _repository.AsQueryable();  // Get the base queryable

//                // Filter if provided using Dynamic LINQ
//                if (request.Filter != null)
//                {
//                    query = query.Where(request.Filter);  // Use Dynamic LINQ Where without casting
//                }

//                // Dynamic sort
//                if (!string.IsNullOrEmpty(request.SortField))
//                    query = query.OrderBy($"{request.SortField} {request.SortDirection}");

//                var totalCount = await _repository.CountAsync();

//                var items = query
//                    .Skip((request.PageIndex - 1) * request.PageSize)
//                    .Take(request.PageSize)
//                    .ToList();

//                var result = _mapper.Map<List<TDto>>(items);
//                var paginated = PaginatedResult<TDto>.Create(result, request.PageIndex, request.PageSize, totalCount);

//                Log.Information("✅ [{TraceId}] Pagination done. Total: {Total}", traceId, totalCount);

//                return BaseResponse<PaginatedResult<TDto>>.Success(paginated, "Query successful", "00");
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex, "❌ [{TraceId}] Error executing paginated query", traceId);
//                return BaseResponse<PaginatedResult<TDto>>.Error("Internal error", "500", traceId);
//            }
//        }
//    }
//}
