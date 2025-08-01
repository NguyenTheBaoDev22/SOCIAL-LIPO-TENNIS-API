using Applications.Interfaces.Repositories;
using AutoMapper;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;

namespace Infrastructure.Persistences.Repositories
{

    public class PaymentTerminalRepository : BaseRepository<PaymentTerminal>, IPaymentTerminalRepository
    {
        public PaymentTerminalRepository(
            AppDbContext context,
            IMapper mapper,
            ICurrentUserService currentUser,
            ILogger<BaseRepository<PaymentTerminal>> logger)
            : base(context, mapper, currentUser, logger)
        {
        }
        public async Task<bool> ExistsActiveSerialNumberAsync(string serialNumber)
        {
            return false;
        }
        public async Task<PaymentTerminal?> FindByCodeAsync(Guid branchId, string terminalCode, CancellationToken cancellationToken = default)
        {
            return null;
        }
    }
}
