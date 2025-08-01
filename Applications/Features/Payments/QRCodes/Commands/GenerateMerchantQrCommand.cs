using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Payments.QRCodes.Commands
{
    public class GenerateMerchantQrCommand : IRequest<BaseResponse<string>>
    {
        public Guid MerchantId { get; set; }  // Lấy từ token middleware
        public Guid MerchantBranchId { get; set; }
        public Guid TerminalId { get; set; }

        [Range(2100, 499999999)] public int Amount { get; set; }
        public string? PurposeOfTransaction { get; set; }
        public string? OrderCode { get; set; }
    }
}
