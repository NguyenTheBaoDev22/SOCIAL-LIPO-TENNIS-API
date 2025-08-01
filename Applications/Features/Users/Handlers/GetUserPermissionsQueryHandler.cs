using Applications.Features.Users.Queries;
using Applications.Interfaces.Repositories;
using MediatR;
using Serilog;
using Shared.Results;

namespace Applications.Features.Users.Handlers
{
    public class GetUserPermissionsQueryHandler : IRequestHandler<GetUserPermissionsQuery, BaseResponse<List<string>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserPermissionsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<List<string>>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            Log.Information("🔍 Getting permissions for UserId: {UserId}, MerchantId: {MerchantId}, BranchId: {BranchId}",
                request.UserId, request.MerchantId, request.MerchantBranchId);

            if (request.UserId == Guid.Empty || request.MerchantId == Guid.Empty)
            {
                Log.Warning("❌ Invalid input: UserId or MerchantId is empty");
                return BaseResponse<List<string>>.Error("Invalid UserId or MerchantId", "400");
            }

            var roleIds = await _unitOfWork.UserRoleAssignmentRepositories
                .GetRoleIdsForUserAsync(request.UserId, request.MerchantId, request.MerchantBranchId);

            if (roleIds == null || !roleIds.Any())
            {
                Log.Information("ℹ️ No roles assigned to user {UserId} for given scope", request.UserId);
                return BaseResponse<List<string>>.Success(new List<string>(), "No roles assigned");
            }

            var permissions = await _unitOfWork.RolePermissionRepositories
                .GetPermissionCodesByRoleIdsAsync(roleIds);

            Log.Information("✅ Retrieved {Count} permissions for UserId: {UserId}", permissions.Count, request.UserId);

            return BaseResponse<List<string>>.Success(permissions, "Permissions retrieved", "00");
        }
    }
}
