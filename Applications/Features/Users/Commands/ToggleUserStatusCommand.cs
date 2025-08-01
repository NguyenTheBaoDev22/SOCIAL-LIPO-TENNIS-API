using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Users.Commands
{
    public class ToggleUserStatusCommand : IRequest<BaseResponse<bool>>
    {
        [Required]
        public Guid UserId { get; set; }

        public bool IsActive { get; set; }
    }
}
