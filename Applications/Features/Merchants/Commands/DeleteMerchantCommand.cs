using MediatR;

namespace Applications.Features.Merchants.Commands
{
    /// <summary>
    /// Command để yêu cầu xóa Merchant
    /// </summary>
    public class DeleteMerchantCommand : IRequest<bool>
    {
        // ID của Merchant cần xóa
        public Guid Id { get; set; }
    }
}
