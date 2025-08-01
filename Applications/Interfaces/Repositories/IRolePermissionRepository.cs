using Core.Entities.AppUsers;
using Shared.Authorization;

namespace Applications.Interfaces.Repositories
{
    public interface IRolePermissionRepository : IBaseRepository<RolePermission>
    {
        Task<List<string>> GetPermissionCodesByRoleIdsAsync(IEnumerable<Guid> roleIds);
        Task<List<PermissionCode>> GetPermissionsByRoleIdAsync(Guid roleId);
    }
}
