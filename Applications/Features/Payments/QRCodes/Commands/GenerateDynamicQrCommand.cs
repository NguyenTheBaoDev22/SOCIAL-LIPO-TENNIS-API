using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Payments.QRCodes.Commands
{
    /// <summary>
    /// Command CQRS để yêu cầu tạo mã QR động cho merchant.
    /// </summary>
    public class GenerateDynamicQrCommand : IRequest<BaseResponse<string>>
    {
        public NapasQrGenReq Request { get; }

        public GenerateDynamicQrCommand(NapasQrGenReq request)
        {
            Request = request;
        }
    }
}
