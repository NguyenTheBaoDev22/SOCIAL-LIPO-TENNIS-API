using Amazon.Runtime;
using Applications.Features.Shops.Inventories.DTOs;
using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Shops.Inventories.Queries
{
    public class GetShopInventoryAuditsQuery : IRequest<BaseResponse<PaginatedResponse<ShopInventoryAuditDto>>>
    {
        public Guid? ProductId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
