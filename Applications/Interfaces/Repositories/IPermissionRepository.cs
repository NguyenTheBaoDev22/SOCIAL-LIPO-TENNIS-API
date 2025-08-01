using Core.Entities.AppUsers;

namespace Applications.Interfaces.Repositories
{
    public interface IPermissionRepository : IBaseRepository<Permission>
    {
        Task<Permission?> GetByCodeAsync(string code);
        Task<List<Permission>> GetAllAsync();
        Task<bool> ExistsByCodeAsync(string code);
    }
}
