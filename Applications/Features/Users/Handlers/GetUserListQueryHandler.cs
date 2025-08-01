using Applications.Features.Users.DTOs;
using Applications.Features.Users.Queries;
using Applications.Interfaces.Repositories;
using AutoMapper;
using Shared.Results;
using Shared.Results.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Core.Entities.AppUsers;

namespace Applications.Features.Users.Handlers
{
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, BaseResponse<PaginatedResult<UserDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<PaginatedResult<UserDto>>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            Log.Information("📄 [GetUserList] Bắt đầu truy vấn danh sách người dùng...");

            //var query = _unitOfWork.UserRepositories.Query().AsNoTracking();
            var query = _unitOfWork.UserRepositories.Query()
                .Include(x => x.UserRoleAssignments)
                    .ThenInclude(x => x.Role)
                .AsNoTracking();
            // 🔍 Filter theo Role
            if (request.RoleId.HasValue)
            {
                var userIds = await _unitOfWork.UserRoleAssignmentRepositories.Query()
                    .Where(x => x.RoleId == request.RoleId)
                    .Select(x => x.UserId)
                    .Distinct()
                    .ToListAsync(cancellationToken);

                query = query.Where(u => userIds.Contains(u.Id));
            }

            // 🔍 Filter theo Tenant / Merchant
            if (request.TenantId.HasValue)
                query = query.Where(x => x.TenantId == request.TenantId);

            if (request.MerchantId.HasValue)
                query = query.Where(x => x.MerchantId == request.MerchantId);

            // 🔍 Filter theo từ khóa
            if (!string.IsNullOrWhiteSpace(request.SearchKeyword))
            {
                var keyword = request.SearchKeyword.Trim().ToLower();
                query = query.Where(u =>
                    u.Username.ToLower().Contains(keyword) ||
                    (u.PhoneNumber != null && u.PhoneNumber.ToLower().Contains(keyword)));
            }

            // ⚙️ Phân trang với ProjectTo
            var paginatedResult = await query
                .ToPaginatedListProjectedAsync<User, UserDto>(
                    _mapper.ConfigurationProvider,
                    request.PageIndex,
                    request.PageSize,
                    cancellationToken
                );

            Log.Information("✅ [GetUserList] Truy vấn thành công. Tổng số: {Count}", paginatedResult.Meta.TotalItems);

            return BaseResponse<PaginatedResult<UserDto>>.Success(paginatedResult);
        }
    }
}
