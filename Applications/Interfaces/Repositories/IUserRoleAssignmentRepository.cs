using Core.Entities.AppUsers;

namespace Applications.Interfaces.Repositories
{
    public interface IUserRoleAssignmentRepository : IBaseRepository<UserRoleAssignment>
    {
        Task<List<Guid>> GetRoleIdsForUserAsync(Guid userId, Guid merchantId, Guid? merchantBranchId);
        Task AssignRoleAsync(Guid userId, Guid roleId, Guid merchantId, Guid? merchantBranchId);

        /// <summary>
        /// Kiểm tra người dùng đã được gán role trong phạm vi merchant/branch chưa
        /// </summary>
        Task<bool> ExistsAsync(Guid userId, Guid roleId, Guid merchantId, Guid? merchantBranchId, CancellationToken cancellationToken = default);
    }
}
