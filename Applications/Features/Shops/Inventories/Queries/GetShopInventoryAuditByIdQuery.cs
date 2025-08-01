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
    public class GetShopInventoryAuditByIdQuery : IRequest<BaseResponse<ShopInventoryAuditDto>>
    {
        public Guid Id { get; set; }
    }
}
