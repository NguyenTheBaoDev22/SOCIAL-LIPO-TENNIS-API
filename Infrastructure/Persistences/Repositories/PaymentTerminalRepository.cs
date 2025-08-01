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
            return await _context.PaymentTerminals
                .AnyAsync(t => t.SerialNumber == serialNumber && !t.IsDeleted);
        }
        public async Task<PaymentTerminal?> FindByCodeAsync(Guid branchId, string terminalCode, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(terminalCode))
                return null;

            return await _context.PaymentTerminals
                .AsNoTracking()
                .Where(t => !t.IsDeleted && t.MerchantBranchId == branchId && t.TerminalCode == terminalCode)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
