using Applications.Features.MerchantBranches.Commands;
using Applications.Interfaces.Repositories;
using MediatR;

namespace Applications.Features.MerchantBranches.Handlers
{
    public class DeleteMerchantBranchCommandHandler : IRequestHandler<DeleteMerchantBranchCommand, bool>
    {
        private readonly IMerchantBranchRepository _repository;

        public DeleteMerchantBranchCommandHandler(IMerchantBranchRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteMerchantBranchCommand request, CancellationToken cancellationToken)
        {
            var merchantBranch = await _repository.GetByIdAsync(request.Id);
            if (merchantBranch == null)
            {
                return false;  // Trả về false nếu không tìm thấy MerchantBranch
            }

            _repository.Delete(merchantBranch);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
