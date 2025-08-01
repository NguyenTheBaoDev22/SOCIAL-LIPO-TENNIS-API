using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Shops.Inventories.Commands
{
    public class DeleteShopInventoryAuditCommand : IRequest<BaseResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}
