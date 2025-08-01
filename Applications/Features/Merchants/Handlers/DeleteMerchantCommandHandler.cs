using Applications.Features.Merchants.Commands;
using Applications.Interfaces.Repositories;
using MediatR;

namespace Applications.Features.Merchants.Handlers
{
    /// <summary>
    /// Handler để xử lý việc xóa Merchant
    /// </summary>
    public class DeleteMerchantCommandHandler : IRequestHandler<DeleteMerchantCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor để inject IUnitOfWork, sử dụng để xóa dữ liệu từ cơ sở dữ liệu
        public DeleteMerchantCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Xử lý DeleteMerchantCommand
        public async Task<bool> Handle(DeleteMerchantCommand request, CancellationToken cancellationToken)
        {
            var merchant = await _unitOfWork.MerchantRepositories.GetByIdAsync(request.Id);
            if (merchant == null)
                return false;

            // Xóa Merchant từ repository và lưu thay đổi vào cơ sở dữ liệu
            _unitOfWork.MerchantRepositories.Delete(merchant);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
