using Core.Entities;

namespace Applications.Interfaces.Repositories
{
    public interface IPaymentTerminalRepository : IBaseRepository<PaymentTerminal>
    {
        Task<bool> ExistsActiveSerialNumberAsync(string serialNumber);
        /// <summary>
        /// Tìm thiết bị thanh toán (Terminal) theo mã chi nhánh và mã thiết bị.
        /// </summary>
        Task<PaymentTerminal?> FindByCodeAsync(Guid branchId, string terminalCode, CancellationToken cancellationToken = default);
    }
}
