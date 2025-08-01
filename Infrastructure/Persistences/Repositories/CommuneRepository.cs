using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Persistences.Repositories
{
    public class CommuneRepository : BaseRepository<Commune>, ICommuneRepository
    {
        private readonly AppDbContext _context;

        public CommuneRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<Commune>> logger)
            : base(context, mapper, currentUser, logger)
        {
            _context = context;
        }

        // Tìm Commune theo mã
        public async Task<Commune> GetByCodeAsync(string code)
        {
            return await _context.Set<Commune>().FirstOrDefaultAsync(c => c.Code == code);
        } // Cập nhật phương thức FindAsync
        public async Task<IQueryable<Commune>> FindAsync(Expression<Func<Commune, bool>> predicate, CancellationToken cancellationToken = default)
        {
            // Trả về IQueryable để có thể thực hiện các phép toán LINQ ngay trên cơ sở dữ liệu
            return AsQueryable().Where(predicate); // Dùng AsQueryable() để tạo ra IQueryable
        }
    }
}
