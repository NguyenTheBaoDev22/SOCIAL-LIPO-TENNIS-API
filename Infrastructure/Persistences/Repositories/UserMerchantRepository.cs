using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities.AppUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using System.Linq.Expressions;

namespace Infrastructure.Persistences.Repositories
{
    public class UserMerchantRepository : BaseRepository<UserMerchant>, IUserMerchantRepository
    {
        private readonly AppDbContext _context;

        public UserMerchantRepository(AppDbContext context, IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<UserMerchant>> logger) 
            : base(context, mapper, currentUser, logger)
        {
            _context = context;
        }

        // Implement FindAsync to match the IBaseRepository interface signature
        public async Task<IEnumerable<UserMerchant>> FindAsync(Expression<Func<UserMerchant, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(predicate).ToListAsync(cancellationToken);  // Ensure it returns Task<IEnumerable<UserMerchant>>
        }
    }


}
