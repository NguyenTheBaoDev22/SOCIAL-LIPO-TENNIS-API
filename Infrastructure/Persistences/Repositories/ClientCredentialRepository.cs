using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;

namespace Infrastructure.Persistences.Repositories
{
    public class ClientCredentialRepository : BaseRepository<ClientCredential>, IClientCredentialRepository
    {
        public ClientCredentialRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<ClientCredential>> logger)
            : base(context, mapper, currentUser, logger)
        {
        }

        public Task<ClientCredential?> GetByClientIdAsync(string clientId)
            => _dbSet.FirstOrDefaultAsync(x => x.ClientId == clientId && !x.IsDeleted);

        public Task<ClientCredential?> ValidateClientAsync(string clientId, string clientSecret)
            => _dbSet.FirstOrDefaultAsync(x => x.ClientId == clientId && x.ClientSecretHash == clientSecret && x.IsActive && !x.IsDeleted);
    }

}
