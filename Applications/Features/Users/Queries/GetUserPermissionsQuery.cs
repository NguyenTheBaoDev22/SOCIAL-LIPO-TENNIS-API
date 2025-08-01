using MediatR;
using Shared.Results;

namespace Applications.Features.Users.Queries
{
    public class GetUserPermissionsQuery : IRequest<BaseResponse<List<string>>>
    {
        public Guid UserId { get; set; }
        public Guid MerchantId { get; set; }
        public Guid? MerchantBranchId { get; set; }
    }

}
