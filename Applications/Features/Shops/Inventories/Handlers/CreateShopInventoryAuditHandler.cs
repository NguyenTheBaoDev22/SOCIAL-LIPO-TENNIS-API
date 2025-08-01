using Applications.Features.Shops.Inventories.Commands;
using Applications.Interfaces.Repositories;
using Core.Entities.Shops;
using MediatR;
using Shared.Interfaces;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Shops.Inventories.Handlers
{
    public class CreateShopInventoryAuditHandler : IRequestHandler<CreateShopInventoryCommand, BaseResponse<Guid>>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserService _currentUser;

        public CreateShopInventoryAuditHandler(IUnitOfWork uow, ICurrentUserService currentUser)
        {
            _uow = uow;
            _currentUser = currentUser;
        }

        public async Task<BaseResponse<Guid>> Handle(CreateShopInventoryCommand request, CancellationToken cancellationToken)
        {
            var audit = new ShopProductInventory
            {
                ProductId = request.ProductId,
                AuditDate = request.AuditDate,
                ExpectedQty = request.ExpectedQty,
                ActualQty = request.ActualQty,
                Reason = request.Reason?.Trim(),
                AuditorName = request.AuditorName?.Trim(),
                TenantId = _currentUser.TenantId??Guid.Empty,
                MerchantId = _currentUser.MerchantId ?? Guid.Empty,
                MerchantBranchId = _currentUser.MerchantBranchId ?? Guid.Empty
            };

            await _uow.ShopProductInventoryRepositories.AddAsync(audit);
            await _uow.SaveChangesAsync();
            return BaseResponse<Guid>.Success(audit.Id);
        }
    }

}
