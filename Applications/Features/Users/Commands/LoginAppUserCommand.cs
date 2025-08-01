using MediatR;
using Shared.DTOs;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Users.Commands
{
    public class LoginAppUserCommand : IRequest<BaseResponse<TokenResult>>
    {
        public string UsernameOrPhone { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
