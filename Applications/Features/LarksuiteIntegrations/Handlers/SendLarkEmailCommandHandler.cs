using Applications.Features.LarksuiteIntegrations.Commands;
using Applications.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Applications.Features.LarksuiteIntegrations.Handlers
{

    public class SendLarkEmailCommandHandler : IRequestHandler<SendLarkEmailCommand, BaseResponse<string>>
    {
        private readonly ILarkEmailService _larkEmailService;

        public SendLarkEmailCommandHandler(ILarkEmailService larkEmailService)
        {
            _larkEmailService = larkEmailService;
        }

        public async Task<BaseResponse<string>> Handle(SendLarkEmailCommand request, CancellationToken cancellationToken)
        {
            return await _larkEmailService.SendEmailAsync(request.Email, request.AccessToken, cancellationToken);
        }
    }

}
