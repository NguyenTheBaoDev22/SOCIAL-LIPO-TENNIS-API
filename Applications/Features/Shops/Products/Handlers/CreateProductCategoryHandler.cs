using Applications.Features.Shops.Products.Commands;
using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities.Shops;
using MediatR;
using Shared.Interfaces;
using Shared.Results;
using Shared.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Applications.Features.Shops.Products.Handlers
{
    /// <summary>
    /// Handler xử lý tạo mới danh mục sản phẩm
    /// </summary>
    public class CreateProductCategoryHandler : IRequestHandler<CreateProductCategoryCommand, BaseResponse<Guid>>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserService _currentUser;

        public CreateProductCategoryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
        {
            _uow = uow;
            _currentUser = currentUser;
        }

        public async Task<BaseResponse<Guid>> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            // Bước 1: Chuẩn hóa mã code: bỏ dấu tiếng Việt, trim, lowercase, bỏ ký tự đặc biệt
            var normalizedCode = StringUtils.NormalizeCode(request.Code);

            // Bước 2: Lấy thông tin Tenant để đảm bảo không trùng lặp mã trong cùng một hệ thống multi-tenant
            var tenantId = _currentUser.TenantId;

            // Bước 3: Kiểm tra trùng mã code trong cùng tenant
            var exists = await _uow.ProductCategoryRepositories
                .AnyAsync(x => x.Code == normalizedCode && x.TenantId == tenantId, cancellationToken);

            if (exists)
            {
                return BaseResponse<Guid>.Error("Mã danh mục đã tồn tại trong hệ thống.");
            }

            // Bước 4: Tạo mới entity - các trường hệ thống sẽ được gán qua interceptor (CreatedBy, TenantId...)
            var category = new ProductCategory
            {
                Code = normalizedCode,
                Name = request.Name?.Trim()
            };

            // Bước 5: Lưu vào database
            await _uow.ProductCategoryRepositories.AddAsync(category, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            // Bước 6: Trả kết quả thành công
            return BaseResponse<Guid>.Success(category.Id);
        }
    }
}
