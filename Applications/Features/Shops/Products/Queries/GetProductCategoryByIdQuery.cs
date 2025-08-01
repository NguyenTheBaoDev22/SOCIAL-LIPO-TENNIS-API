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
    public class GetProductCategoryByIdQuery : IRequest<BaseResponse<ProductCategoryDto>>
    {
        public Guid Id { get; set; }
    }
}
