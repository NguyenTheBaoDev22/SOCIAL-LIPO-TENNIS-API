using MediatR;
using Shared.Results;
using System.ComponentModel.DataAnnotations;

namespace Applications.Features.Merchants.Commands
{
    namespace Applications.Features.Merchants.Commands
    {
        public class ApproveMerchantCommand : IRequest<BaseResponse<bool>>
        {
            [Required]
            public Guid MerchantBranchId { get; set; }
        }
    }
}
