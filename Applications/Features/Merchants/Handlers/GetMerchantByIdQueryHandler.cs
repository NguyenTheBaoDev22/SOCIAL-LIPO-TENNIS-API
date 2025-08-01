using Applications.Features.Merchants.DTOs;
using Applications.Features.Merchants.Queries;
using Applications.Interfaces.Repositories;
using MediatR;

namespace Applications.Features.Merchants.Handlers
{
    public class GetMerchantByIdQueryHandler : IRequestHandler<GetMerchantByIdQuery, MerchantDto>
    {
        private readonly IMerchantRepository _merchantRepository;

        // Inject repository vào constructor
        public GetMerchantByIdQueryHandler(IMerchantRepository merchantRepository)
        {
            _merchantRepository = merchantRepository;
        }

        public async Task<MerchantDto> Handle(GetMerchantByIdQuery request, CancellationToken cancellationToken)
        {
            var merchant = await _merchantRepository.GetByIdAsync(request.Id);
            if (merchant == null)
            {
                return null; // Hoặc bạn có thể throw exception tùy ý
            }

            // Chuyển đổi entity thành DTO
            var merchantDto = new MerchantDto
            {
                Id = merchant.Id,
                MerchantName = merchant.MerchantName,
                BusinessRegistrationNo = merchant.BusinessRegistrationNo,
                Email = merchant.PrimaryEmail,
                MerchantCode = merchant.MerchantCode,
                // Thêm các thuộc tính khác của MerchantDto
            };

            return merchantDto;
        }
    }
}
