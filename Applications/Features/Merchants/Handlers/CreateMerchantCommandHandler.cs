using Applications.Features.Merchants.Commands;
using Applications.Features.Merchants.DTOs;
using Applications.Interfaces.Repositories;
using Core.Entities;
using MediatR;

namespace Applications.Features.Merchants.Handlers
{
    /// <summary>
    /// Handler để xử lý việc tạo mới Merchant
    /// </summary>
    public class CreateMerchantCommandHandler : IRequestHandler<CreateMerchantCommand, MerchantDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor để inject IUnitOfWork, sử dụng để thêm dữ liệu vào cơ sở dữ liệu
        public CreateMerchantCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Xử lý CreateMerchantCommand
        public async Task<MerchantDto> Handle(CreateMerchantCommand request, CancellationToken cancellationToken)
        {
            var merchant = new Merchant
            {
                MerchantName = request.MerchantName,
                BusinessRegistrationNo = request.BusinessRegistrationNo,
                BusinessAddress = request.BusinessAddress,
                OwnerName = request.OwnerName,
                PrimaryEmail = request.Email,
                PrimaryPhone = request.Phone,
                MerchantType = request.MerchantType,
            };

            // Thêm Merchant vào repository và lưu vào cơ sở dữ liệu
            await _unitOfWork.MerchantRepositories.AddAsync(merchant);
            await _unitOfWork.SaveChangesAsync();

            // Trả về MerchantDto với thông tin Merchant đã tạo
            return new MerchantDto
            {
                Id = merchant.Id,
                MerchantCode = merchant.MerchantCode, // Mã Merchant tự động tạo
                MerchantName = merchant.MerchantName,
                Email = merchant.PrimaryEmail
            };
        }
    }
}
