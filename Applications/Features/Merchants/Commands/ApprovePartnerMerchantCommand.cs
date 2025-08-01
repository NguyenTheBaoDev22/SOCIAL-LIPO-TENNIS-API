using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Merchants.Commands
{
   
    public class ApprovePartnerMerchantCommand : IRequest<BaseResponse<bool>>
    {
        [Required]
        public Guid MerchantBranchId { get; set; }
    }
}
