using Core.Entities.AppUsers;

namespace Applications.Interfaces.Repositories
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<bool> ExistsAsync(Guid roleId);
        Task<Role?> GetByIdAsync(Guid roleId);
    }
}
