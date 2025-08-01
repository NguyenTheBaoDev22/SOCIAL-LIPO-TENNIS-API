using Amazon.Runtime;
using Applications.Features.Shops.Products.DTOs;
using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Shops.Products.Queries
{
    public class GetProductCategoriesQuery : IRequest<BaseResponse<PaginatedResponse<ProductCategoryDto>>>
    {
        public string? Keyword { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
