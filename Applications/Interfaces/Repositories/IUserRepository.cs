using Core.Entities.AppUsers;
using System.Linq.Expressions;

namespace Applications.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<User?> GetByIdWithRolesAsync(Guid userId, CancellationToken cancellationToken);
        Task<User?> FindByPhoneAsync(string phoneNumber, CancellationToken cancellationToken = default);
    }
}
