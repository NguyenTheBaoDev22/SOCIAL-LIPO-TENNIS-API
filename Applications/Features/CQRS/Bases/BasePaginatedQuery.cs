using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.CQRS.Bases
{
    public abstract class BasePaginatedQuery<TDto> : IRequest<BaseResponse<PaginatedResult<TDto>>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortField { get; set; }
        public string? SortDirection { get; set; } = "asc";
        public Expression<Func<object, bool>>? Filter { get; set; }
    }
}
