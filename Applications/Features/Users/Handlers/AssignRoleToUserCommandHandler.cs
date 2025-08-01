using Applications.Features.Users.Commands;
using Applications.Interfaces.Repositories;
using Core.Entities.AppUsers;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Results;

namespace Applications.Features.Users.Handlers
{
    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, BaseResponse<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AssignRoleToUserCommandHandler> _logger;

        public AssignRoleToUserCommandHandler(IUnitOfWork unitOfWork, ILogger<AssignRoleToUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BaseResponse<string>> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("🔁 Assigning RoleId {RoleId} to UserId {UserId} (TenantId: {TenantId}, MerchantId: {MerchantId}, BranchId: {BranchId})",
                request.RoleId, request.UserId, request.TenantId, request.MerchantId, request.MerchantBranchId);

            // 1. Validate user existence
            var user = await _unitOfWork.UserRepositories.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("❌ User not found: {UserId}", request.UserId);
                return BaseResponse<string>.Error("User not found", "User_NotFound");
            }

            // 2. Validate role existence
            var role = await _unitOfWork.RoleRepositories.GetByIdAsync(request.RoleId, cancellationToken);
            if (role == null)
            {
                _logger.LogWarning("❌ Role not found: {RoleId}", request.RoleId);
                return BaseResponse<string>.Error("Role not found", "Role_NotFound");
            }

            // 3. Check existing assignment
            var exists = await _unitOfWork.UserRoleAssignmentRepositories.ExistsAsync(
                request.UserId, request.RoleId, request.MerchantId, request.MerchantBranchId, cancellationToken);

            if (exists)
            {
                _logger.LogWarning("⚠️ Duplicate assignment for UserId {UserId} to RoleId {RoleId} (MerchantId: {MerchantId}, BranchId: {BranchId})",
                    request.UserId, request.RoleId, request.MerchantId, request.MerchantBranchId);
                return BaseResponse<string>.Error("User already assigned to this role in the given scope.", "Role_Assignment_Exists");
            }

            // 4. Create role assignment
            var assignment = new UserRoleAssignment
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                RoleId = request.RoleId,
                TenantId = request.TenantId,
                MerchantId = request.MerchantId,
                MerchantBranchId = request.MerchantBranchId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.UserRoleAssignmentRepositories.AddAsync(assignment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("✅ Role assigned successfully: UserId {UserId}, RoleId {RoleId}, AssignmentId {AssignmentId}",
                request.UserId, request.RoleId, assignment.Id);

            return BaseResponse<string>.Success(assignment.Id.ToString(), "Role assigned successfully", "00");
        }
    }
}
