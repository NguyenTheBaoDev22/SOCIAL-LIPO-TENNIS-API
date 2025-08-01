using Core.Entities;

namespace Applications.Interfaces.Repositories
{
    public interface ICommuneRepository : IBaseRepository<Commune>
    {
        // Thêm các phương thức tùy chỉnh cho `Commune`
        Task<Commune> GetByCodeAsync(string code);
    }
}
