using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;

namespace Infrastructure.Persistences.Repositories
{
    public class ProvinceRepository : BaseRepository<Province>, IProvinceRepository
    {
        public ProvinceRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<Province>> logger)
            : base(context, mapper, currentUser, logger)
        {
        }

        // Cập nhật phương thức GetAllAsync để trả về IQueryable<Province>
        public async Task<IQueryable<Province>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            // Trả về IQueryable để có thể tiếp tục linh động với dữ liệu từ cơ sở dữ liệu
            return _context.Set<Province>().AsQueryable();  // Trả về IQueryable từ DbContext
        }

        public async Task<Province> GetByCodeAsync(string code)
        {
            return await _context.Set<Province>().FirstOrDefaultAsync(p => p.Code == code);
        }
    }
}
