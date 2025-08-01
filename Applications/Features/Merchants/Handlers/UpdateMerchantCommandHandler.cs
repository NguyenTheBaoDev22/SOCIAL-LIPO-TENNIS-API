using Applications.Features.Merchants.Commands;
using Applications.Features.Merchants.DTOs;
using Applications.Interfaces.Repositories;
using MediatR;

namespace Applications.Features.Merchants.Handlers
{
    /// <summary>
    /// Handler để xử lý việc cập nhật Merchant
    /// </summary>
    public class UpdateMerchantCommandHandler : IRequestHandler<UpdateMerchantCommand, MerchantDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor để inject IUnitOfWork, sử dụng để cập nhật dữ liệu vào cơ sở dữ liệu
        public UpdateMerchantCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Xử lý UpdateMerchantCommand
        public async Task<MerchantDto> Handle(UpdateMerchantCommand request, CancellationToken cancellationToken)
        {
            var merchant = await _unitOfWork.MerchantRepositories.GetByIdAsync(request.Id);
            if (merchant == null)
                return null;

            // Cập nhật thông tin Merchant
            merchant.MerchantName = request.MerchantName;
            merchant.BusinessRegistrationNo = request.BusinessRegistrationNo;
            merchant.BusinessAddress = request.BusinessAddress;
            merchant.OwnerName = request.OwnerName;
            merchant.PrimaryEmail = request.Email;
            merchant.PrimaryPhone = request.Phone;
            merchant.MerchantType = request.MerchantType;
            merchant.IsActive = request.IsActive;

            // Cập nhật Merchant vào repository và lưu vào cơ sở dữ liệu
            _unitOfWork.MerchantRepositories.Update(merchant);
            await _unitOfWork.SaveChangesAsync();

            // Trả về MerchantDto đã cập nhật
            return new MerchantDto
            {
                Id = merchant.Id,
                MerchantCode = merchant.MerchantCode, // Mã Merchant
                MerchantName = merchant.MerchantName,
                Email = merchant.PrimaryEmail
            };
        }
    }
}
