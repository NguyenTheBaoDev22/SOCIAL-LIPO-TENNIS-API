using Core.Entities;

namespace Applications.Interfaces.Repositories
{
    public interface IProvinceRepository : IBaseRepository<Province>
    {
        Task<Province> GetByCodeAsync(string code);
        Task<IQueryable<Province>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
