using Applications.Features.MerchantBranches.Commands;
using Applications.Features.MerchantBranches.Dtos;
using Applications.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Applications.Features.MerchantBranches.Handlers
{
    public class UpdateMerchantBranchCommandHandler : IRequestHandler<UpdateMerchantBranchCommand, MerchantBranchDto>
    {
        private readonly IMerchantBranchRepository _repository;
        private readonly IMapper _mapper;

        public UpdateMerchantBranchCommandHandler(IMerchantBranchRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<MerchantBranchDto> Handle(UpdateMerchantBranchCommand request, CancellationToken cancellationToken)
        {
            var merchantBranch = await _repository.GetByIdAsync(request.Id);
            if (merchantBranch == null)
            {
                return null; // Hoặc bạn có thể throw exception tùy ý
            }

            // Cập nhật thông tin từ command
            _mapper.Map(request, merchantBranch);

            await _repository.SaveChangesAsync();

            return _mapper.Map<MerchantBranchDto>(merchantBranch);
        }
    }
}
